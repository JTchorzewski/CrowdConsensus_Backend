using Domain.Model;

namespace Domain.Interfaces;

public interface IFinancialRepository
{
    Task AddAsync(FinancialData data);
}