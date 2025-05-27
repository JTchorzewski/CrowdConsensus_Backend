using System.Globalization;
using System.Text.Json;
using Domain.Model;
using HtmlAgilityPack;

namespace Application.Services;

public static class DataScraper
{
    public static async Task ScrapeFinancialDataAsync(string ticker, string filePath, int companyId)
    {
        var url = $"https://www.biznesradar.pl/raporty-finansowe-rachunek-zyskow-i-strat/{ticker},Q";
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);

        var results = new List<FinancialData>();

        var headerNodes = doc.DocumentNode.SelectNodes("//tr[1]/th[position() > 1]");
        if (headerNodes == null)
        {
            Console.WriteLine("Nie znaleziono nagłówków kolumn z datami.");
            return;
        }

        var dates = headerNodes
            .Select(th => th.InnerText.Trim().Split('\n')[0].Trim('\"', ' ', '\t', '\r'))
            .ToList();

        var netProfitRow = doc.DocumentNode.SelectSingleNode("//tr[@data-field='IncomeNetProfit']");

        if (netProfitRow == null)
        {
            Console.WriteLine("Nie znaleziono wierszy z danymi.");
            return;
        }

        var netProfitCells = netProfitRow.SelectNodes("td[position() > 1]");
        

        if (netProfitCells == null || netProfitCells.Count != dates.Count)
        {
            Console.WriteLine("Niezgodna liczba kolumn z danymi lub datami.");
            return;
        }

        for (int i = 0; i < dates.Count; i++)
        {
            decimal? netProfit = ParseValueFromCell(netProfitCells[i]);

            if (netProfit.HasValue)
            {
                results.Add(new FinancialData
                {
                    Id = i + 1,
                    CompanyId = companyId,
                    NetProfit = netProfit ?? 0,
                    RaportDate = dates[i]
                });
            }
        }

        var json = JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, json);

        Console.WriteLine($"Zapisano {results.Count} rekordów do pliku {filePath}");
    }

    private static decimal? ParseValueFromCell(HtmlNode cell)
    {
        var spanValue = cell.SelectSingleNode(".//span[@class='pv']/span");
        if (spanValue != null)
        {
            var rawText = spanValue.InnerText.Trim().Replace(" ", "");
            if (decimal.TryParse(rawText, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                return value;
        }
        return null;
    }
}