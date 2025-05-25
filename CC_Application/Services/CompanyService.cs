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

    public ListCompanyRaportForListVm GetAllCompanyRaportsForList(int page, int pageSize, string q)
    {
        var raports = _financialRepository.GetAllCompaniesRaports();
        
        if (!string.IsNullOrWhiteSpace(q))
        {
            raports = raports.Where(r => r.CompanyName.Contains(q, StringComparison.OrdinalIgnoreCase));
        }

        var totalCount = raports.Count();
        
        var paginatedRaports = raports
            .OrderBy(r => r.CompanyName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new ListCompanyRaportForListVm
        {
            CompanyRaportList = paginatedRaports.Select(raport => new CompanyRaportForListVm
            {
                Id = raport.Id,
                CompanyName = raport.CompanyName,
                NetProfit = raport.NetProfit,
                Revenue = raport.Revenue,
                RaportDate = raport.RaportDate
            }).ToList(),
            TotalCount = totalCount
        };

        return result;
    }
}