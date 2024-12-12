using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TypeOfActivityController : ControllerBase
{
    private readonly ITypeOfActivityService _typeOfActivityService;
    public TypeOfActivityController(ITypeOfActivityService typeOfActivityService)
    {
        _typeOfActivityService = typeOfActivityService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Type(Guid id)
    {
        var typeOfActivity = await _typeOfActivityService.GetTypeAsync(id);

        if (typeOfActivity.Data == null) return BadRequest(typeOfActivity.Message);

        return Ok(typeOfActivity.Data);
    }


    [HttpGet("Types")]
    public async Task<IActionResult> Types()
    {
        var types = await _typeOfActivityService.GetTypesAsync();

        return Ok(types);
    }



    [Authorize(Policy = "AdminOnly")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] TypeOfActivityModel typeModel)
    {
        var response = await _typeOfActivityService.AddTypeAsync(typeModel);
        if (response.Data == null) return BadRequest($"{response.Message}");

        return Ok(response.Data);
    }



    [Authorize(Policy = "AdminOnly")]
    [HttpPost("CreateRange")]
    public async Task<IActionResult> CreateTypes([FromBody] List<TypeOfActivityModel> typeModels)
    {
        var response = await _typeOfActivityService.AddTypesRangeAsync(typeModels);
        if (response.Item1 == null) return BadRequest($"Failed to create type of activity\n{response.Item2!.Title} already exist!");

        return Ok(response.Item1);
    }



    [Authorize(Policy = "AdminOnly")]
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateTypeOfActivityModel typeModel)
    {
        var result = await _typeOfActivityService.UpdateTypeAsync(typeModel);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }



    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromHeader] Guid id)
    {
        var result = await _typeOfActivityService.DeleteTypeAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}