using PersonalAccount.API.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PersonalAccount.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "StaffOnly")]
public class StaffImageController : ControllerBase
{
    private readonly IStaffImagesService _staffImagesService;
    public StaffImageController(IStaffImagesService staffImagesService)
    {
        _staffImagesService = staffImagesService;
    }


    [HttpPost("Add")]
    public async Task<ActionResult<string>> Add([FromHeader] Guid staffId, List<IFormFile> files)
    {
        var result = await _staffImagesService.AddImagesAsync(staffId, files);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }



    [HttpPut("UpdateImages")]
    public async Task<ActionResult<string>> UpdateImages([FromHeader] Guid staffId, List<IFormFile> newFiles)
    {
        var result = await _staffImagesService.UpdateImagesAsync(staffId,newFiles);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }


    [HttpPut("UpdateImage")]
    public async Task<ActionResult<string>> UpdateImage([FromHeader] string lastFileName, IFormFile newFile)
    {
        var result = await _staffImagesService.UpdateImageAsync(lastFileName, newFile);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }


    [HttpPatch("{id:guid}/IsPageImage")]
    public async Task<ActionResult<string>> UpdateIsPageImage(Guid id)
    {
        var result = await _staffImagesService.UpdateIsPageImageAsync(id);

        if (result.Data == null)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }


    [HttpDelete("Delete")]
    public async Task<ActionResult<string>> Delete([FromHeader] Guid id)
    {
        var result = await _staffImagesService.DeleteImageAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}