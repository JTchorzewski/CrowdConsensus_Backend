using Domain.Model;

namespace Application.Interfaces;

public interface IFinancialDataService
{
    Task<FinancialData> ScrapeAndSaveDataAsync(string id, string companyName);
}