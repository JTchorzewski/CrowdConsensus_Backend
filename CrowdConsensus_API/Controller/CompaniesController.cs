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
        
        [HttpGet]
        public IActionResult Index([FromQuery] int page = 1, [FromQuery] int pageSize = 40, [FromQuery] string q = "")
        {
            var result = _companyService.GetAllCompanyRaportsForList(page, pageSize, q);

            return Ok(new {
                totalCount = result.TotalCount,
                items = result.CompanyRaportList
            });
        }
        [HttpPost("import-json-folder")]
        public async Task<IActionResult> ImportJsonFromFolder([FromQuery] string folder)
        {
            if (string.IsNullOrEmpty(folder))
                return BadRequest("Ścieżka folderu jest wymagana.");

            var count = await _jsonDataImporter.ImportAllFromFolderAsync(folder);
            return Ok($"✅ Zaimportowano {count} rekordów z folderu: {folder}");
        }
    }

}