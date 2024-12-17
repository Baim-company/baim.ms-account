using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Controllers;


[Authorize]
[ApiController]
[Route("[controller]")]
public class UserPhotoController : ControllerBase
{

    private readonly IUserPhotoService _userPhotoService;
    private readonly AgileDbContext _agileDbContext;

    public UserPhotoController(IUserPhotoService userPhotoService,
        AgileDbContext agileDbContext)
    {
        _userPhotoService = userPhotoService;
        _agileDbContext = agileDbContext;
    }
    


    [HttpGet("User/Id/{userId}")]
    public async Task<IActionResult> GetFileByUserId(Guid userId)
    {
        var result = await _userPhotoService.GetFileByUserIdAsync(userId);
        if (result.Data == null)
            return NotFound(result.Message);

        var file = result.Data;
        return File(file.FileContent, file.ContentType, file.FileName);
    }



    [HttpPut("User/Id/{userId}")]
    public async Task<ActionResult<string>> UpdateFile(Guid userId, [FromHeader] string imagePath)
    {
        var result = await _userPhotoService.UpdateFilePathAsync(userId, imagePath);
        if (!result.Data)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }



    [HttpDelete("User/Id/{userId}")]
    public async Task<ActionResult<string>> DeleteFile(Guid userId,[FromHeader]string imagePath)
    {
        var result = await _userPhotoService.DeleteFileAsync(userId,imagePath);
        if (!result.Data)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}