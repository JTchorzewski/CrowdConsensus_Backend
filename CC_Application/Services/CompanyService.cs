using Application.Interfaces;
using Application.ViewModels;
using Domain.Interfaces;
using Infrastructure.Repositories;

namespace Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public ListCompanyForListVm GetAllCompanyForList()
    {
        var companies = _companyRepository.GetAllCompaniesInfo();
        ListCompanyForListVm result = new ListCompanyForListVm();
        result.CompanyList = new List<CompanyForListVm>();
        string groupsList = "";
        foreach (var company in companies)
        {
            var groups = company.CompanyToGroupConnection.Where(s => s.CompanyId == company.Id);
            foreach (var group in groups)
            {
                groupsList = group + ", " + groupsList;
            }

            var companyVm = new CompanyForListVm()
            {
                Id = company.Id,
                CompanyName = company.Name,
                Groups = groupsList,
                NextRaportDate = company.NextRaportDate
            };
            result.CompanyList.Add(companyVm);
            groupsList = "";
        }
        return result;
    }
}