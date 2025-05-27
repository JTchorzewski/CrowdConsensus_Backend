using Microsoft.AspNetCore.Mvc;
using Application.ViewModels;
using Application.Interfaces;

namespace CrowdConsensus_API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IJsonDataImporter _jsonDataImporter;
        public CompaniesController(ICompanyService companyService , IJsonDataImporter jsonDataImporter)
        {
            _companyService = companyService;
            _jsonDataImporter = jsonDataImporter;
        }
        
        [HttpGet("{companyId}")]
        public IActionResult Index(int companyId, [FromQuery] int page = 1, [FromQuery] int pageSize = 40, [FromQuery] string q = "")
        {
            var result = _companyService.GetAllCompanyRaportsForList(page, pageSize, q, companyId);

            return Ok(new {
                totalCount = result.TotalCount,
                items = result.CompanyRaportList
            });
        }
        [HttpGet("CompanyList")]
        public IActionResult CompanyList([FromQuery] int page = 1, [FromQuery] int pageSize = 40, [FromQuery] string q = "")
        {
            var result = _companyService.GetAllCompanyNamesForList(page, pageSize, q);

            return Ok(new {
                totalCount = result.TotalCount,
                items = result.CompanyNamesList
            });
        }
        [HttpPost("import-json-folder")]
        public async Task<IActionResult> ImportJsonFromFolder([FromQuery] string folder)
        {
            if (string.IsNullOrEmpty(folder))
                return BadRequest("Ścieżka folderu jest wymagana.");

            var count = await _jsonDataImporter.ImportAllFromFolderAsync(folder);
            return Ok($"aimportowano {count} rekordów z folderu: {folder}");
        }
    }

}