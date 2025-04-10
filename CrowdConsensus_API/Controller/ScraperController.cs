using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrowdConsensus_API.Controller;

[ApiController]
[Route("[controller]")]
public class ScraperController : ControllerBase
{
    private readonly IScraperService _scraperService;
    public ScraperController(IScraperService scraperService)
    {
        _scraperService = scraperService;
    }
        
    [HttpPost("scrape-data/{symbol}")]
    public async Task<IActionResult> ScrapeData(string symbol)
    {
        await _scraperService.ScrapeAndAddCompanyData(symbol);
        return Ok("Data scraped and added.");
    }
}