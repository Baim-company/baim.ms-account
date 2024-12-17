using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Data.Dtos.Clients;
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
    public async Task<ActionResult<TypeOfActivityDto>> Type(Guid id)
    {
        var typeOfActivity = await _typeOfActivityService.GetTypeAsync(id);

        if (typeOfActivity.Data == null) return BadRequest(typeOfActivity.Message);

        return Ok(typeOfActivity.Data);
    }



    [HttpGet("Types")]
    public async Task<ActionResult<List<TypeOfActivityDto>>> Types()
    {
        var types = await _typeOfActivityService.GetTypesAsync();

        return Ok(types);
    }



    [Authorize(Policy = "AdminOnly")]
    [HttpPost("Create")]
    public async Task<ActionResult<string>> Create([FromBody] TypeOfActivityDto typeOfActivityDto)
    {
        var response = await _typeOfActivityService.AddTypeAsync(typeOfActivityDto);
        if (response.Data == null) return BadRequest($"{response.Message}");

        return Ok(response.Message);
    }



    [Authorize(Policy = "AdminOnly")]
    [HttpPost("CreateRange")]
    public async Task<ActionResult<string>> CreateTypes([FromBody] List<TypeOfActivityDto> typeOfActivityDtos)
    {
        var response = await _typeOfActivityService.AddTypesRangeAsync(typeOfActivityDtos);
        if (response.Data == null) return BadRequest(response.Message);

        return Ok(response.Message);
    }


    [Authorize(Policy = "AdminOnly")]
    [HttpPatch("Update/{id}")]
    public async Task<ActionResult<string>> UpdateTitle(Guid id, [FromBody] TypeOfActivityDto typeOfActivityDto)
    {
        var result = await _typeOfActivityService.UpdateTypeAsync(id, typeOfActivityDto);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }


    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("Delete")]
    public async Task<ActionResult<string>> Delete([FromHeader] Guid id)
    {
        var result = await _typeOfActivityService.DeleteTypeAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}