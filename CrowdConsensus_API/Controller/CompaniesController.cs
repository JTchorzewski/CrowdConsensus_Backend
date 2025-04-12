using Microsoft.AspNetCore.Mvc;
using Application.ViewModels;
using Application.Interfaces;

namespace CrowdConsensus_API.Controller
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var model = _companyService.GetAllCompanyForList();
            return Ok(model);
        }
        [HttpGet]
        public IActionResult NettoIncome()
        {
            var model = _companyService.GetAllCompanyNettoForList();
            return Ok(model);
        }
    }

}