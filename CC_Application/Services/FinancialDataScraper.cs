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

        decimal? netProfit = null;
        decimal? revenue = null;
        DateTime? reportDate = null;

        foreach (var row in rows)
        {
            var cells = row.SelectNodes("td");

            if (cells == null || cells.Count < 2)
                continue;

            if (cells.Count >= 3)
            {
                // Wiersze z danymi finansowymi (3 kolumny)
                var label = cells[1].InnerText.Trim();
                var value = cells[2].InnerText.Trim();

                if (string.Equals(label, "Zysk netto", StringComparison.OrdinalIgnoreCase) && netProfit == null)
                {
                    if (decimal.TryParse(value.Replace(" ", "").Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var profit))
                        netProfit = profit;
                }

                if (string.Equals(label, "Przychody ze sprzedaży", StringComparison.OrdinalIgnoreCase) && revenue == null)
                {
                    if (decimal.TryParse(value.Replace(" ", "").Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var rev))
                        revenue = rev;
                }
            }
            else if (cells.Count == 2)
            {
                // Wiersze z metadanymi (2 kolumny)
                var label = cells[0].InnerText.Trim();
                var value = cells[1].InnerText.Trim();

                if (string.Equals(label, "Data sporządzenia", StringComparison.OrdinalIgnoreCase))
                {
                    if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                        reportDate = parsedDate;
                }
            }
        }

        if (netProfit == null)
            throw new Exception("Nie znaleziono informacji o zysku netto.");

        if (revenue == null)
            throw new Exception("Nie znaleziono informacji o przychodach ze sprzedaży.");

        if (reportDate == null)
            throw new Exception("Nie znaleziono daty sporządzenia raportu.");

        return new FinancialData
        {
            CompanyName = companyName,
            NetProfit = netProfit.Value,
            Revenue = revenue.Value,
            RaportDate = DateTime.SpecifyKind(reportDate.Value, DateTimeKind.Utc)
        };
    }
}