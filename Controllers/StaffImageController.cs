using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos.Staffs;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Controllers;

[ApiController]
//[Authorize(Policy = "StaffOnly")]
[Route("[controller]")]
public class StaffImageController : ControllerBase
{
    private readonly IStaffImagesService _staffImagesService;
    public StaffImageController(IStaffImagesService staffImagesService)
    {
        _staffImagesService = staffImagesService;
    }


    [HttpPost("Add")]
    public async Task<IActionResult> Add([FromHeader] Guid staffId, [FromBody] List<ImageModel> imageModels)
    {
        var result = await _staffImagesService.AddImagesAsync(staffId, imageModels);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }



    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] List<UpdateImageModel> updateImageModels)
    {
        var result = await _staffImagesService.UpdateImagesAsync(updateImageModels);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }
     


    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromHeader] Guid id)
    {
        var result = await _staffImagesService.DeleteImageAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}