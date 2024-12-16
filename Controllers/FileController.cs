using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Services.Abstractions;


namespace PersonalAccount.API.Controllers;


[ApiController]
[Authorize(Policy = "AdminOnly")]
[Route("[controller]")]
public class FileController : ControllerBase
{

    private readonly string _baseImageUrl;
    private readonly IFileService _fileService;

    public FileController(IFileService fileService,
        IConfiguration configuration)
    {
        _fileService = fileService;
        _baseImageUrl = configuration["BaseImageUrl"]
            ?? throw new Exception("BaseImageUrl is not configured");
    }


    [HttpGet("Name/{fileName}")]
    public async Task<ActionResult<FileContentResult>> GetFile(string fileName)
    {
        var result = await _fileService.GetFileByNameAsync(fileName);
        if (result.Data == null) return NotFound(result.Message);
        var file = result.Data;

        return File(file.FileContent, file.ContentType, file.FileName);
    }



    [HttpPost("UploadFile")]
    public async Task<ActionResult<string>> UploadFile(IFormFile file)
    {
        var result = await _fileService.CreateFileAsync(file);

        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [HttpPut("Change")]
    public async Task<ActionResult<string>> UpdateFile([FromHeader] string fileName, IFormFile newFile)
    {
        var result = await _fileService.UpdateFileAsync(fileName, newFile);

        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Data);
    }


    [HttpDelete("Name/{fileName}")]
    public ActionResult<string> DeleteFile(string fileName)
    {
        var result = _fileService.DeleteFile(fileName);
        if (!result.Data) return NotFound(result.Message);

        return Ok(result.Message);
    }
}