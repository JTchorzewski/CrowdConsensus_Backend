using Domain.Interfaces;
using Domain.Model;

namespace Infrastructure.Repositories;

public class FinancialRepository: IFinancialRepository
{
    private readonly DataContext _dataContext;

    public FinancialRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddAsync(FinancialData data)
    {
        _dataContext.FinancialData.Add(data);
        await _dataContext.SaveChangesAsync();
    }
}