using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Agiles.Projects;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Agiles.Projects;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> Project(Guid id)
    {
        var response = await _projectService.GetProjectAsync(id);

        if (response.Data == null) return BadRequest(response.Message);

        return Ok(response.Data);
    }



    //[Authorize(Policy = "AdminAndStaffOnly")]
    [HttpGet("Projects")]
    public async Task<ActionResult<PagedResponse<Project>>> Projects([FromQuery] PaginationParameters paginationParameters
        , [FromQuery] string? onSearch
        , [FromQuery] string? onProduct
        , [FromQuery] bool sortByDateCreated)
    {
        var projects = await _projectService.GetProjcetsAsync(paginationParameters, onSearch, onProduct, sortByDateCreated);

        return Ok(projects);
    }



    [HttpGet("Projects/Company/{companyId}")]
    public async Task<ActionResult<PagedResponse<Project>>> CompanyProjects(Guid companyId, [FromQuery] PaginationParameters paginationParameters
        , [FromQuery] string? onSearch
        , [FromQuery] string? onProduct
        , [FromQuery] bool sortByDateCreated)
    {
        var projects = await _projectService.GetProjcetsByCompanyAsync(companyId, paginationParameters, onSearch, onProduct, sortByDateCreated);

        return Ok(projects);
    }



    [HttpGet("Projects/Participant/{participantId}")]
    public async Task<ActionResult<PagedResponse<Project>>> ParticipantProjects(Guid participantId, [FromQuery] PaginationParameters paginationParameters
        , [FromQuery] string? onSearch
        , [FromQuery] string? onProduct
        , [FromQuery] bool sortByDateCreated)
    {
        var projects = await _projectService.GetProjcetsByParticipationAsync(participantId, paginationParameters, onSearch, onProduct, sortByDateCreated);

        return Ok(projects);
    }




    //[Authorize(Policy = "AdminAndStaffOnly")]
    [HttpPost("Create")]
    public async Task<ActionResult<string>> Create([FromBody] ProjectModel projectModel)
    {
        var response = await _projectService.AddProjectAsync(projectModel);
        if (response.Data == null) return BadRequest($"{response.Message}");

        return Ok(response.Message);
    }




    //[Authorize(Policy = "AdminAndStaffOnly")]
    [HttpPut("Update")]
    public async Task<ActionResult<string>> Update([FromBody] UpdateProjectModel projectModel)
    {
        var result = await _projectService.UpdateProjectAsync(projectModel);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }


    
    //[Authorize(Policy = "AdminAndStaffOnly")]
    [HttpPut("Complete")]
    public async Task<ActionResult<string>> Complete([FromHeader] Guid id)
    {
        var result = await _projectService.CompleteProjectAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }



    //[Authorize(Policy = "AdminOnly")]
    [HttpPut("MakePublic")]
    public async Task<ActionResult<string>> MakePublic([FromHeader] Guid id)
    {
        var result = await _projectService.MakeProjectPublicAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }


    

    //[Authorize(Policy = "AdminAndStaffOnly")]
    [HttpDelete("Delete")]
    public async Task<ActionResult<string>> Delete([FromHeader]Guid id)
    {
        var result = await _projectService.DeleteProjectAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}