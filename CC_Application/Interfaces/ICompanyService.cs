using Application.ViewModels;

namespace Application.Interfaces;

public interface ICompanyService
{
    public ListCompanyRaportForListVm GetAllCompanyRaportsForList(int page, int pageSize, string q, int companyId);
    public ListCompanyNamesForListVm GetAllCompanyNamesForList(int page, int pageSize, string q);
}