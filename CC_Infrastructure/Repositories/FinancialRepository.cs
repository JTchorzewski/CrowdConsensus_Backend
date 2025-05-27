using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FinancialRepository: IFinancialRepository
{
    private readonly DataContext _dataContext;

    public FinancialRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddAsync(FinancialData data)
    {
        _dataContext.FinancialData.Add(data);
        await _dataContext.SaveChangesAsync();
    }
    public IQueryable<FinancialData> GetAllCompaniesRaports(int companyId)
    {
        return _dataContext.FinancialData.Include(f => f.Company).Where(q => q.CompanyId == companyId);
    }
    public IQueryable<Company> GetAllCompaniesNames()
    {
        return _dataContext.Companies;
    }
}