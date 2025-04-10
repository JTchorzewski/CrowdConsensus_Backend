using Application.Interfaces;
using Domain.Interfaces;
using Domain.Model;

namespace Application.Services;

public class FinancialDataService : IFinancialDataService
{
    private readonly IFinancialRepository _repository;

    public FinancialDataService(IFinancialRepository repository)
    {
        _repository = repository;
    }

    public async Task<FinancialData> ScrapeAndSaveDataAsync(string id, string companyName)
    {
        var data = await FinancialDataScraper.ScrapeNetProfitAsync(id, companyName);
        await _repository.AddAsync(data);
        return data;
    }
}