using Domain.Model;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly DataContext _dataContext;

    public CompanyRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IQueryable<Company> GetAllCompaniesInfo()
    {
        return _dataContext.Companies.Include(s => s.CompanyToGroupConnection).ThenInclude(c => c.Group);
    }

    public IQueryable<Group> GetAllGroups()
    {
        return _dataContext.Groups;
    }
}