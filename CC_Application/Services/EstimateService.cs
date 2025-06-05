using Application.Interfaces;
using Application.ViewModels;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class EstimateService : IEstimateService
{
    private readonly IEstimateRepository _estimateRepository;
    private readonly IFinancialRepository _companyRepository;

    public EstimateService(IEstimateRepository estimateRepository, IFinancialRepository financialRepository)
    {
        _estimateRepository = estimateRepository;
        _companyRepository = financialRepository;
    }

    public async Task<string?> AddEstimateAsync(string userId, int companyId, float estimateValue)
    {
        var existing = await _estimateRepository.GetByUserAndCompanyAsync(userId, companyId);
        if (existing != null)
            return "Już dodałeś estymację dla tej spółki.";

        var company = await _companyRepository.GetByIdAsync(companyId);
        if (company == null)
            return "Spółka nie istnieje.";

        var estimate = new Estimate
        {
            CompanyId = companyId,
            CompanyName = company.Name,
            EstimateValue = estimateValue,
            AppUserId = userId
        };

        await _estimateRepository.AddAsync(estimate);
        await _estimateRepository.SaveChangesAsync();

        return null;
    }
    public async Task<ListEstimateForListVm> GetEstimatesForListAsync(int page, int pageSize, string? search = null)
    {
        // Pobierz wszystkie estymacje z repozytorium (tu trzeba mieć repo metodę zwracającą IQueryable<Estimate>)
        var query = _estimateRepository.GetAllEstimates();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e => e.CompanyName.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        var totalCount = await query.CountAsync();

        var estimatesPage = await query
            .OrderBy(e => e.CompanyName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var vm = new ListEstimateForListVm
        {
            TotalCount = totalCount,
            EstimatesForListVm = estimatesPage.Select(e => new EstimateForListVm
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                CompanyName = e.CompanyName,
                EstimateValue = e.EstimateValue
            }).ToList()
        };

        return vm;
    }
}