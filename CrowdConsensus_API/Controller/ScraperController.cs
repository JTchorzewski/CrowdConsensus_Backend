using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialDataController : ControllerBase
    {
        [HttpPost("scrape")]
        public async Task<IActionResult> ScrapeDynamic([FromQuery] string ticker, [FromQuery] string company)
        {
            if (string.IsNullOrWhiteSpace(ticker) || string.IsNullOrWhiteSpace(company))
                return BadRequest("Ticker i nazwa firmy są wymagane.");

            var sanitizedFileName = $"{ticker.ToLowerInvariant()}.json";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), sanitizedFileName);

            Console.WriteLine($"🚀 Scraping: {ticker} - {company}");
            await DataScraper.ScrapeFinancialDataAsync(ticker, company, filePath);
            Console.WriteLine($"✅ Gotowe: {sanitizedFileName}");

            return Ok(new { message = $"Scraped {company}", file = sanitizedFileName });
        }
    }
}