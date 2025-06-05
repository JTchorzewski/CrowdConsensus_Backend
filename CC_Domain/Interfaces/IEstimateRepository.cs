using Domain.Model;

namespace Domain.Interfaces;

public interface IEstimateRepository
{
    Task<Estimate?> GetByUserAndCompanyAsync(string userId, int companyId);
    Task AddAsync(Estimate estimate);
    Task SaveChangesAsync();
    public IQueryable<Estimate> GetAllEstimates();
}