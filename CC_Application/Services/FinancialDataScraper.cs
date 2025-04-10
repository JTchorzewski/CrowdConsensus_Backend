using System.Globalization;
using Domain.Model;
using HtmlAgilityPack;

namespace Application.Services;

public static class FinancialDataScraper
{
    public static async Task<FinancialData> ScrapeNetProfitAsync(string id, string companyName)
    {
        var url = $"https://espiebi.pap.pl/node/{id}";
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);
        
        var rows = doc.DocumentNode.SelectNodes("//tr");

        if (rows == null)
            throw new Exception("Nie znaleziono żadnych wierszy tabeli.");

        foreach (var row in rows)
        {
            var cells = row.SelectNodes("td");
            if (cells != null && cells.Count >= 3)
            {
                var label = cells[1].InnerText.Trim();
                var value = cells[2].InnerText.Trim();

                if (string.Equals(label, "Zysk netto", StringComparison.OrdinalIgnoreCase))
                {
                    var cleanedValue = value
                        .Replace(" ", "")
                        .Replace(",", ".");

                    if (decimal.TryParse(cleanedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var profit))
                    {
                        return new FinancialData
                        {
                            CompanyName = companyName,
                            NetProfit = profit,
                            RetrievedAt = DateTime.UtcNow
                        };
                    }
                    else
                    {
                        throw new Exception("Nie udało się sparsować wartości zysku netto.");
                    }
                }
            }
        }

        throw new Exception("Nie znaleziono informacji o zysku netto.");
    }
}