using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos.Agiles.ProjectSubTickets;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectSubTiketController : ControllerBase
{
    private readonly IProjectSubTicketService _projectSubTiketService;
    public ProjectSubTiketController(IProjectSubTicketService projectSubTiketService)
    {
        _projectSubTiketService = projectSubTiketService;
    }

    //[Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> ProjectSubTicket(Guid id)
    {
        var response = await _projectSubTiketService.GetProjectSubTicketAsync(id);

        if (response == null) return BadRequest($"Error! Project sub-ticket with id: {id} doesn't exist!");

        return Ok(response);
    }


    //[Authorize]
    [HttpGet("ProjectSubTickets")]
    public async Task<IActionResult> ProjectSubTickets()
    {
        var response = await _projectSubTiketService.GetProjectSubTicketsAsync();

        if (response == null) return BadRequest("Error! Project sub-tickets are null here!");

        return Ok(response);
    }



    //[Authorize(Policy = "AdminAndStaffOnly")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ProjectSubTiketModel model)
    {
        var result = await _projectSubTiketService.CreateProjectSubTicketAsync(model);

        if (result.Data == null) return BadRequest($"{result.Message}");

        return Ok(result.Data);
    }

     

    //[Authorize(Policy = "AdminAndStaffOnly")]
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateProjectSubTiketModel model)
    {
        var result = await _projectSubTiketService.UpdateProjectSubTicketAsync(model);

        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }



    //[Authorize(Policy = "AdminAndStaffOnly")]
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromHeader] Guid id)
    {
        var result = await _projectSubTiketService.DeleteProjectSubTicketAsync(id);

        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}