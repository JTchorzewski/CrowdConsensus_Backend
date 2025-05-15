using Application.Interfaces;
using Application.ViewModels;
using Domain.Interfaces;
using Infrastructure.Repositories;

namespace Application.Services;

public class CompanyService : ICompanyService
{
    private readonly IFinancialRepository _financialRepository;

    public CompanyService(IFinancialRepository financialRepository)
    {
        _financialRepository = financialRepository;
    }

    public ListCompanyRaportForListVm GetAllCompanyRaportsForList()
    {
        var raports = _financialRepository.GetAllCompaniesRaports();
        ListCompanyRaportForListVm result = new ListCompanyRaportForListVm();
        result.CompanyRaportList = new List<CompanyRaportForListVm>();
        foreach (var raport in raports)
        {
            var companyVm = new CompanyRaportForListVm()
            {
                Id = raport.Id,
                CompanyName = raport.CompanyName,
                NetProfit = raport.NetProfit,
                Revenue = raport.Revenue,
                RaportDate = raport.RaportDate
            };
            result.CompanyRaportList.Add(companyVm);
        }

        return result;
    }
}