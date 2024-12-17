using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos.Agiles.ProjectTickets;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectTicketController : ControllerBase
{
    private readonly IProjectTicketService _projectTicketService;

    public ProjectTicketController(IProjectTicketService projectTicketService)
    {
        _projectTicketService = projectTicketService;
    }


    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> ProjectTicket(Guid id)
    {
        var response = await _projectTicketService.GetProjectTicketByIdAsync(id);

        if (response == null) return BadRequest($"Error! Project ticket with id: {id} doesn't exist!");

        return Ok(response);
    }



    [Authorize]
    [HttpGet("ProjectTickets")]
    public async Task<IActionResult> ProjectTickets()
    {
        var response = await _projectTicketService.GetProjectTicketsAsync();

        if (response == null) return BadRequest("Error! Project tickets are null here!");

        return Ok(response);
    }




    [Authorize(Policy = "StaffOnly")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ProjectTiketModel model)
    {
        var result = await _projectTicketService.CreateProjectTicketAsync(model);

        if (result.Data == null) return BadRequest($"{result.Message}");

        return Ok(result.Data);
    }



    [Authorize(Policy = "StaffOnly")]
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateProjectTiketModel model)
    {
        var result = await _projectTicketService.UpdateProjectTicketAsync(model);

        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }



    [Authorize(Policy = "StaffOnly")]
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromHeader] Guid id)
    {
        var result = await _projectTicketService.DeleteProjectTicketAsync(id);

        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}