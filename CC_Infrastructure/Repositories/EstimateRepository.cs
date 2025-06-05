using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EstimateRepository : IEstimateRepository
{
    private readonly DataContext _context;

    public EstimateRepository(DataContext context)
    {
        _context = context;
    }

    public IQueryable<Estimate> GetAllEstimates()
    {
        return _context.Estimates;
    }
    public async Task<Estimate?> GetByUserAndCompanyAsync(string userId, int companyId)
    {
        return await _context.Estimates
            .FirstOrDefaultAsync(e => e.CompanyId == companyId && e.AppUserId == userId);
    }

    public async Task AddAsync(Estimate estimate)
    {
        await _context.Estimates.AddAsync(estimate);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}