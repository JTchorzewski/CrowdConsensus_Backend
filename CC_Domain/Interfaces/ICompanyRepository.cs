using Domain.Model;

namespace Domain.Interfaces;

public interface ICompanyRepository
{
    public IQueryable<Company> GetAllCompaniesInfo();
    public IQueryable<Group> GetAllGroups();
}