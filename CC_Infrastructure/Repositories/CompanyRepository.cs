using Domain.Model;
using Domain.Interfaces;

namespace Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly DataContext _dataContext;

    public CompanyRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IQueryable<Company> GetCompanies()
    {
        return _dataContext.Companies;
    }
}