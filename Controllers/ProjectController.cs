using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Agiles.Projects;
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
    public async Task<IActionResult> Project(Guid id)
    {
        var response = await _projectService.GetProjectAsync(id);

        if (response.Data == null) return BadRequest(response.Message);

        return Ok(response.Data);
    }



    //[Authorize(Policy = "AdminAndStaffOnly")]
    [HttpGet("Projects")]
    public async Task<IActionResult> Projects([FromQuery] PaginationParameters paginationParameters
        , [FromQuery] string? onSearch
        , [FromQuery] string? onProduct
        , [FromQuery] bool sortByDateCreated)
    {
        var projects = await _projectService.GetProjcetsAsync(paginationParameters, onSearch, onProduct, sortByDateCreated);

        return Ok(projects);
    }



    [HttpGet("Projects/Company/{companyId}")]
    public async Task<IActionResult> CompanyProjects(Guid companyId, [FromQuery] PaginationParameters paginationParameters
        , [FromQuery] string? onSearch
        , [FromQuery] string? onProduct
        , [FromQuery] bool sortByDateCreated)
    {
        var projects = await _projectService.GetProjcetsByCompanyAsync(companyId, paginationParameters, onSearch, onProduct, sortByDateCreated);

        return Ok(projects);
    }



    [HttpGet("Projects/Participant/{participantId}")]
    public async Task<IActionResult> ParticipantProjects(Guid participantId, [FromQuery] PaginationParameters paginationParameters
        , [FromQuery] string? onSearch
        , [FromQuery] string? onProduct
        , [FromQuery] bool sortByDateCreated)
    {
        var projects = await _projectService.GetProjcetsByParticipationAsync(participantId, paginationParameters, onSearch, onProduct, sortByDateCreated);

        return Ok(projects);
    }




    //[Authorize(Policy = "AdminAndStaffOnly")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ProjectModel projectModel)
    {
        var response = await _projectService.AddProjectAsync(projectModel);
        if (response.Data == null) return BadRequest($"{response.Message}");

        return Ok(response.Data);
    }




    //[Authorize(Policy = "AdminAndStaffOnly")]
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateProjectModel projectModel)
    {
        var result = await _projectService.UpdateProjectAsync(projectModel);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Data);
    }


    
    //[Authorize(Policy = "AdminAndStaffOnly")]
    [HttpPut("Complete")]
    public async Task<IActionResult> Complete([FromHeader] Guid id)
    {
        var result = await _projectService.CompleteProjectAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }



    //[Authorize(Policy = "AdminOnly")]
    [HttpPut("MakePublic")]
    public async Task<IActionResult> MakePublic([FromHeader] Guid id)
    {
        var result = await _projectService.MakeProjectPublicAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }


    

    //[Authorize(Policy = "AdminAndStaffOnly")]
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromHeader]Guid id)
    {
        var result = await _projectService.DeleteProjectAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}