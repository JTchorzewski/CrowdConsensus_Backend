using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CrawlerController : ControllerBase
    {
        [HttpPost("run")]
        public async Task<IActionResult> RunCrawler([FromQuery] int startId = 300000, [FromQuery] int endId = 999999, [FromQuery] int threads = 70)
        {
            try
            {
                var fileName = $"crawler_result_{DateTime.UtcNow:yyyyMMddHHmmss}.json";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

                _ = Task.Run(async () =>
                {
                    await FinancialDataParallelScraper.RunParallelScrapingAsync(filePath, startId, endId, threads);
                });

                return Accepted(new
                {
                    Message = $"Crawler uruchomiony w tle. Wyniki zostaną zapisane do {fileName}"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}