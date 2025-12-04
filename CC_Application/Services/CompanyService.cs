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
        var companies = _financialRepository.GetAllCompaniesNames();

        if (!string.IsNullOrWhiteSpace(q))
        {
            companies = companies.Where(r => r.Name.Contains(q, StringComparison.OrdinalIgnoreCase));
        }

        var totalCount = companies.Count();

        var paginatedCompanies = companies
            .OrderBy(r => r.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        
        decimal sum = 0;
        decimal prediction = 0;
        decimal prediction2 = 0;
        
        var result = new ListCompanyNamesForListVm
        {
            CompanyNamesList = paginatedCompanies.Select(company =>
            {
                var latestReport = company.FinancialData?
                    .OrderByDescending(fd => ParseRaportDate(fd.RaportDate))
                    .FirstOrDefault();
                foreach (var companyx in companies)
                {
                    var financialData = companyx.FinancialData;
                    foreach (var netProfit in financialData)
                    {
                         sum += netProfit.NetProfit;
                    }
                    prediction = sum / (financialData.Count()+100000);
                    prediction2 = prediction / latestReport.NetProfit;
                }
                return new CompanyNamesForListVm
                {
                    Id = company.Id,
                    CompanyName = company.Name,
                    NewestNetProfit = latestReport.NetProfit,
                    NewestRaportDate = latestReport?.RaportDate,
                    NewestPrediction = prediction2
                };
            }).ToList(),
            TotalCount = totalCount
        };

        return result;
    }
    private int ParseRaportDate(string raportDate)
    {
        if (string.IsNullOrEmpty(raportDate)) return 0;

        var parts = raportDate.Split('/');
        if (parts.Length != 2) return 0;

        if (int.TryParse(parts[0], out var year) &&
            parts[1].StartsWith("Q") &&
            int.TryParse(parts[1].Substring(1), out var quarter))
        {
            return year * 10 + quarter;
        }

        return 0;
    }
}