using Microsoft.AspNetCore.Mvc;
using Application.ViewModels;
using Application.Interfaces;

namespace CrowdConsensus_API.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class CompaniesController(ICompanyService companyService) : ControllerBase
    {
        private readonly ICompanyService _companyService = companyService;

        [HttpGet]
        public IActionResult GetCompanies()
        {
            ListCompanyForListVm companies = _companyService.GetAllCompanyForList();

            if (companies == null || companies.CompanyList == null)
            {
                return NotFound();
            }

            return Ok(companies);
        }
    }

}