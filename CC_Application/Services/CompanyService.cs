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

    public ListCompanyRaportForListVm GetAllCompanyRaportsForList(int page, int pageSize, string q, int companyId)
    {
        var raports = _financialRepository.GetAllCompaniesRaports(companyId);
        
        if (!string.IsNullOrWhiteSpace(q))
        {
            raports = raports.Where(r => r.Company.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
        }

        var totalCount = raports.Count();
        
        var paginatedRaports = raports
            .OrderBy(r => r.Company.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new ListCompanyRaportForListVm
        {
            CompanyRaportList = paginatedRaports.Select(raport => new CompanyRaportForListVm
            {
                Id = raport.Id,
                CompanyName = raport.Company.Name,
                NetProfit = raport.NetProfit,
                Revenue = raport.Revenue,
                RaportDate = raport.RaportDate
            }).ToList(),
            TotalCount = totalCount
        };

        return result;
    }
    
    public ListCompanyNamesForListVm GetAllCompanyNamesForList(int page, int pageSize, string q)
    {
        var company = _financialRepository.GetAllCompaniesNames();
        
        if (!string.IsNullOrWhiteSpace(q))
        {
            company = company.Where(r => r.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
        }

        var totalCount = company.Count();
        
        var paginatedRaports = company
            .OrderBy(r => r.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new ListCompanyNamesForListVm
        {
            CompanyNamesList = paginatedRaports.Select(company => new CompanyNamesForListVm
            {
                Id = company.Id,
                CompanyName = company.Name,
            }).ToList(),
            TotalCount = totalCount
        };

        return result;
    }
}