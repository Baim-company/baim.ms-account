using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos.Agiles.ProjectTasks;
using PersonalAccount.API.Services.Abstractions;

[ApiController]
[Route("[controller]")]
public class ProjectTaskController : ControllerBase
{
    private readonly IProjectTaskService _projectTaskService;

    public ProjectTaskController(IProjectTaskService projectTaskService)
    {
        _projectTaskService = projectTaskService;
    }


    //[Authorize]
    [HttpPost("CreateTicketTask")]
    public async Task<IActionResult> CreateTicketTask([FromBody] ProjectTicketTaskModel model)
    {
        if (!ModelState.IsValid) return BadRequest("Invalid data.");

        var result = await _projectTaskService.CreateProjectTaskOnTicketAsync(model);

        if (result.Data == null) return BadRequest(result.Message);

        return CreatedAtAction(nameof(CreateTicketTask), new { id = result.Data.Id }, result.Data);
    }



    //[Authorize]
    [HttpPost("CreateSubTicketTask")]
    public async Task<IActionResult> CreateSubTicketTask([FromBody] ProjectSubTicketTaskModel model)
    {
        if (!ModelState.IsValid) return BadRequest("Invalid data.");

        var result = await _projectTaskService.CreateProjectTaskOnSubTicketAsync(model);

        if (result.Data == null) return BadRequest(result.Message);

        return CreatedAtAction(nameof(CreateSubTicketTask), new { id = result.Data.Id }, result.Data);
    }



    //[Authorize]
    [HttpPut("ChangeStateProjectTask")]
    public async Task<IActionResult> ChangeStateProjectTaskAsync([FromHeader]Guid id, [FromBody] string doneState)
    {
        if (!ModelState.IsValid) return BadRequest("Invalid data.");

        var result = await _projectTaskService.ChangeStateProjectTaskAsync(id, doneState);

        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Data);
    }



    //[Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectTaskById(Guid id)
    {
        if (id == Guid.Empty) return BadRequest("Invalid task ID.");

        var response = await _projectTaskService.GetProjectTaskByIdAsync(id);

        if (response.Data == null) return NotFound(response.Message);

        return Ok(response.Data);
    }



    //[Authorize]
    [HttpGet("ProjectTasks/Project/{projectId}")]
    public async Task<IActionResult> GetProjectTasksByProjectId(Guid projectId)
    {
        if (projectId == Guid.Empty) return BadRequest("Invalid project ID.");

        var response = await _projectTaskService.GetProjectTasksByProjectIdAsync(projectId);

        if (response.Data == null || !response.Data.Any()) return NotFound(response.Message);

        return Ok(response.Data);
    }



    //[Authorize]
    [HttpGet("ProjectTasks/SubTicket/{projectSubTicketId}")]
    public async Task<IActionResult> GetProjectTasksBySubTicketId(Guid projectSubTicketId)
    {
        if (projectSubTicketId == Guid.Empty) return BadRequest("Invalid sub-ticket ID.");

        var response = await _projectTaskService.GetProjectTasksBySubTicketIdAsync(projectSubTicketId);

        if (response.Data == null || !response.Data.Any()) return NotFound(response.Message);

        return Ok(response.Data);
    }




    //[Authorize]
    [HttpGet("ProjectTasks/Ticket/{projectTicketId}")]
    public async Task<IActionResult> GetProjectTasksByTicketId(Guid projectTicketId)
    {
        if (projectTicketId == Guid.Empty) return BadRequest("Invalid ticket ID.");

        var response = await _projectTaskService.GetProjectTasksByTicketIdAsync(projectTicketId);

        if (response.Data == null || !response.Data.Any()) return NotFound(response.Message);

        return Ok(response.Data);
    }



    //[Authorize]
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateProjectTask([FromBody] UpdateProjectTaskModel updateModel)
    {
        if (!ModelState.IsValid) return BadRequest("Invalid data.");

        var response = await _projectTaskService.UpdateProjectTaskAsync(updateModel);

        if (response.Data == null) return BadRequest(response.Message);

        return Ok(response.Data);
    }



    //[Authorize]
    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteProjectTask(Guid id)
    {
        if (id == Guid.Empty) return BadRequest("Invalid task ID.");

        var response = await _projectTaskService.DeleteProjectTaskAsync(id);

        if (response.Data == null) return NotFound(response.Message);
        
        return Ok(response.Message);
    }
}