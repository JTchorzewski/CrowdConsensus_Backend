using System.Text.Json;
using Application.Interfaces;
using Domain.Model;
using Infrastructure;

namespace Application.Services;

public class JsonDataImporter : IJsonDataImporter
{
    private readonly DataContext _context;

    public JsonDataImporter(DataContext context)
    {
        _context = context;
    }

    public async Task<int> ImportAllFromFolderAsync(string folderPath)
    {
        if (!Directory.Exists(folderPath))
            return 0;

        var jsonFiles = Directory.GetFiles(folderPath, "*.json");
        var totalImported = 0;

        foreach (var file in jsonFiles)
        {
            try
            {
                var json = await File.ReadAllTextAsync(file);
                var records = JsonSerializer.Deserialize<List<FinancialData>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (records != null && records.Count > 0)
                {
                    await _context.FinancialData.AddRangeAsync(records);
                    await _context.SaveChangesAsync();
                    totalImported += records.Count;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy pliku {Path.GetFileName(file)}: {ex.Message}");
            }
        }

        return totalImported;
    }
}