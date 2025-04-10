using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrowdConsensus_API.Controller;

[ApiController]
[Route("api/[controller]")]
public class FinancialDataController : ControllerBase
{
    private readonly IFinancialDataService _service;

    public FinancialDataController(IFinancialDataService service)
    {
        _service = service;
    }

    [HttpPost("scrape/{companyName}/{id}")]
    public async Task<IActionResult> ScrapeNetProfit(string id, string companyName)
    {
        try
        {
            var data = await _service.ScrapeAndSaveDataAsync(id, companyName);
            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message, inner = ex.InnerException?.Message });
        }
    }
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("API działa");
    }
}