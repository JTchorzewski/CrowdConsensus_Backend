using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Text.Json;
using Domain.Model;
using HtmlAgilityPack;

namespace Application.Services;

public static class FinancialDataParallelScraper
{
    private static readonly string[] TargetCompanies = new[]
    {
        "PKO BANK POLSKI S.A.", "ORLEN S.A.", "PZU SA", "BANK PEKAO S.A.", "DINO POLSKA S.A.",
        "ALLEGRO.EU S.A.", "LPP", "SANTANDER BANK POLSKA SA", "KGHM POLSKA MIEDŹ SA", "CCC",
        "mBank S.A.", "ALIOR BANK S.A.", "Zabka", "KETY", "BUDIMEX", "PGE POLSKA GRUPA ENERGETYCZNA S.A.",
        "KRUK S.A."
    };

    public static async Task RunParallelScrapingAsync(string filePath, int startId, int endId, int maxDegreeOfParallelism)
    {
        var results = new ConcurrentBag<FinancialData>();
        var stopwatch = Stopwatch.StartNew();
        var totalIds = endId - startId + 1;
        var checkedIds = 0;
        var errors = 0;
        var successes = 0;

        var progressLock = new object();

        await Parallel.ForEachAsync(Enumerable.Range(startId, totalIds), new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async (id, ct) =>
        {
            try
            {
                var result = await TryScrapeAsync(id.ToString());
                if (result != null)
                {
                    results.Add(result);
                    Interlocked.Increment(ref successes);
                }
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref errors);
            }
            finally
            {
                var done = Interlocked.Increment(ref checkedIds);

                if (done % 10 == 0 || done == totalIds)
                {
                    lock (progressLock)
                    {
                        var elapsed = stopwatch.Elapsed;
                        var avgTimePerId = elapsed.TotalSeconds / done;
                        var remainingTime = TimeSpan.FromSeconds(avgTimePerId * (totalIds - done));

                        Console.WriteLine($"[Progress] Checked: {done}/{totalIds} | Successes: {successes} | Errors: {errors} | Elapsed: {elapsed:mm\\:ss} | ETA: {remainingTime:mm\\:ss}");
                    }
                }
            }
        });

        stopwatch.Stop();

        var json = JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, json);

        Console.WriteLine($"✅ Scraping zakończony. Wyniki zapisane do {filePath}");
    }

    private static async Task<FinancialData?> TryScrapeAsync(string id)
    {
        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };

        var url = $"https://espiebi.pap.pl/node/{id}";
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);

        var rows = doc.DocumentNode.SelectNodes("//tr");

        if (rows == null)
            return null;

        string? companyName = null;
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
                var label = cells[0].InnerText.Trim();
                var value = cells[1].InnerText.Trim();

                if (string.Equals(label, "Data sporządzenia", StringComparison.OrdinalIgnoreCase))
                {
                    if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                        reportDate = parsedDate;
                }

                if (string.Equals(label, "Symbol Emitenta", StringComparison.OrdinalIgnoreCase))
                {
                    companyName = value;
                }
            }
        }

        if (companyName == null || !TargetCompanies.Contains(companyName))
            return null;

        if (netProfit == null || revenue == null || reportDate == null)
            return null;

        return new FinancialData
        {
            CompanyName = companyName,
            NetProfit = netProfit.Value,
            Revenue = revenue.Value,
            RaportDate = ""
        };
    }
}