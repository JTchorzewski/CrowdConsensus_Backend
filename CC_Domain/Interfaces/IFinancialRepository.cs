using Domain.Model;

namespace Domain.Interfaces;

public interface IFinancialRepository
{
    Task AddAsync(FinancialData data);
    public IQueryable<FinancialData> GetAllCompaniesRaports();
}