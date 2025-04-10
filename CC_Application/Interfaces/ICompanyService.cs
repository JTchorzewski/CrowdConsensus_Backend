using Application.ViewModels;

namespace Application.Interfaces;

public interface ICompanyService
{
    public ListCompanyForListVm GetAllCompanyForList();
    public ListCompanyNettoForListVm GetAllCompanyNettoForList();
}