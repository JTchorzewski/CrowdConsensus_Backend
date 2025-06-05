using Application.ViewModels;

namespace Application.Interfaces;

public interface IEstimateService
{
    Task<string?> AddEstimateAsync(string userId, int companyId, float estimateValue);
    Task<ListEstimateForListVm> GetEstimatesForListAsync(int page, int pageSize, string? search = null);
}