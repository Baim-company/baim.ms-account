using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Services.Abstractions;


namespace PersonalAccount.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "AdminOnly")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost("Create")]
    public async Task<ActionResult<string>> Create([FromBody] ExternalUserDto externalUserDto)
    {
        var response = await _adminService.AddAdminAsync(externalUserDto);
        if (response.Data == null) return BadRequest($"{response.Message}");

        return StatusCode(200);
    }
}