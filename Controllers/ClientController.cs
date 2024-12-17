using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Services.Abstractions;


namespace PersonalAccount.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{

    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }



    [Authorize]
    [HttpGet("User/{userId}")]
    public async Task<ActionResult<Client>> GetClientByUserId(Guid userId)
    {
        var response = await _clientService.GetClientByUserIdAsync(userId);

        if (response.Data == null) return BadRequest(response.Message);

        return Ok(response.Data);
    }


    [Authorize]
    [HttpGet("Client/{id}")]
    public async Task<ActionResult<Client>> Client(Guid id)
    {
        var response = await _clientService.GetClientAsync(id);

        if (response.Data == null) return BadRequest(response.Message);

        return Ok(response.Data);
    }


    [Authorize]
    [HttpGet("Clients")]
    public async Task<ActionResult<PagedResponse<List<Client>>>> Clients(
        [FromQuery] PaginationParameters paginationParameters, 
        [FromQuery] string? onFilter)
    {
        var clients = await _clientService.GetFilteredClientsAsync(paginationParameters, onFilter);

        return Ok(clients);
    }



    [Authorize(Policy = "AdminOnly")]
    [HttpPost("Create/ClientAdmin")]
    public async Task<IActionResult> CreateAdmin([FromBody] ExternalUserDto externalUserDto)
    {
        var response = await _clientService.AddClientAdminAsync(externalUserDto);
        if (response.Data == null) return BadRequest($"{response.Message}");

        return Ok(response.Data);
    }


    [Authorize(Policy = "HighPriorityOnly")]
    [HttpPost("Create/Client")]
    public async Task<IActionResult> Create([FromBody] ExternalUserCompanyDto externalUserCompanyDto)
    {
        var response = await _clientService.AddClientAsync(externalUserCompanyDto.User, externalUserCompanyDto.CompanyId);
        if (response.Data == null) return BadRequest($"{response.Message}");

        return Ok(response.Data);
    }



    [Authorize(Roles = "User,UserAdmin")]
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserModel updateUserModel)
    {
        var result = await _clientService.UpdateClientDataAsync(updateUserModel);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Data);
    }


    [Authorize(Policy = "AdminOnly")]
    [HttpPatch("Clients/{id}/Activate")]
    public async Task<ActionResult<string>> ActivateClient(Guid id)
    {
        var result = await _clientService.UpdateClientActiveStatusAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }

}