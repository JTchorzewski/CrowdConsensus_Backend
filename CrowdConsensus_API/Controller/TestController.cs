using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("secured-endpoint")]
    [Authorize]
    public IActionResult GetSecuredData()
    {
        return Ok(new { Message = "Dostęp przyznany! Jesteś zalogowany i masz prawidłowy JWT." });
    }

    [HttpGet("open-endpoint")]
    public IActionResult GetOpenData()
    {
        return Ok(new { Message = "To jest endpoint otwarty, nie wymaga JWT." });
    }
}