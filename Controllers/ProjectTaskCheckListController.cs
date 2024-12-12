using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos.Agiles.CheckItems;
using PersonalAccount.API.Services;

namespace PersonalAccount.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectTaskCheckListController : ControllerBase
{
    private readonly IProjectTaskCheckListService _checkListService;

    public ProjectTaskCheckListController(IProjectTaskCheckListService checkListService)
    {
        _checkListService = checkListService;
    }

    //[Authorize]
    [HttpGet("{projectTaskId}")]
    public async Task<IActionResult> GetCheckListByTaskId(Guid projectTaskId)
    {
        var response = await _checkListService.GetCheckListByTaskIdAsync(projectTaskId);

        if (response.Data == null)
        {
            return NotFound(new { Message = response.Message });
        }

        return Ok(response);
    }



    //[Authorize]
    [HttpPost("Create")]
    public async Task<IActionResult> CreateCheckList([FromBody] ProjectTaskCheckListCreateModel checkListCreateModel)
    {
        var response = await _checkListService.CreateCheckListAsync(checkListCreateModel);

        if (response.Data == null)
        {
            return BadRequest(new { Message = response.Message });
        }

        return CreatedAtAction(nameof(GetCheckListByTaskId), new { projectTaskId = checkListCreateModel.ProjectTaskId }, response);
    }



    //[Authorize]
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateCheckList([FromBody] ProjectTaskCheckListModel checkListModel)
    {
        var response = await _checkListService.UpdateCheckListAsync(checkListModel);

        if (response.Data == null)
        {
            return BadRequest(new { Message = response.Message });
        }

        return Ok(response);
    }



    //[Authorize]
    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteCheckList([FromHeader] Guid id)
    {
        var response = await _checkListService.DeleteCheckListAsync(id);

        if (!response.Data) return NotFound(new { Message = response.Message });

        return NoContent();
    }
}
