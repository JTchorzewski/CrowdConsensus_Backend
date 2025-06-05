using System.Security.Claims;
using Application.Interfaces;
using Domain.Model;
using Domain.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CrowdConsensus_API.Controller;
[Authorize]
[Route("api/[controller]")]
public class EstimateController : ControllerBase
{
    private readonly IEstimateService _estimateService;
    private readonly UserManager<AppUser> _userManager;

    public EstimateController(IEstimateService estimateService, UserManager<AppUser> userManager)
    {
        _estimateService = estimateService;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> AddEstimate([FromBody] AddEstimateDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        await _estimateService.AddEstimateAsync(userId, dto.CompanyId, dto.EstimateValue);
        return Ok("Estymacja dodana.");
    }
    
    [HttpGet("show")]
    public async Task<IActionResult> Show([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
    {
        var vm = await _estimateService.GetEstimatesForListAsync(page, pageSize, search);
        return Ok(vm);
    }
}