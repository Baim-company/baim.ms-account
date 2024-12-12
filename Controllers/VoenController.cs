using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class VoenController : ControllerBase
{
    private readonly IVoenService _voenService;
    public VoenController(IVoenService voenService)
    {
        _voenService = voenService;
    }

    [HttpGet]
    public async Task<IActionResult> GetVoen([FromHeader] string voen)
    {
        var company = await _voenService.GetVoenDetailsAsync(voen);
        return Ok(company);
    }
}