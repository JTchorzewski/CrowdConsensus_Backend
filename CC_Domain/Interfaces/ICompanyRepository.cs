using Domain.Model;

namespace Domain.Interfaces;

public interface ICompanyRepository
{
    public IQueryable<Spolki> GetAllCompaniesInfo();
    public IQueryable<Group> GetAllGroups();
    public IQueryable<FinancialData> GetAllCompaniesNettoIncome();
}