using Domain.Interfaces;
using Domain.Model;

namespace Infrastructure.Repositories;

public class FinancialRepository: IFinancialRepository
{
    private readonly DataContext _context;

    public FinancialRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddAsync(FinancialData data)
    {
        _context.FinancialData.Add(data);
        await _context.SaveChangesAsync();
    }
}