﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Data.Dtos.Staffs;
using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Dtos.Staffs;
using PersonalAccount.API.Models.Entities.Staffs;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Controllers;


[ApiController]
[Route("[controller]")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;
    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }




    [HttpGet("{id}")]
    public async Task<ActionResult<Staff>> Staff(Guid id)
    {
        var response = await _staffService.GetStaffAsync(id);

        if (response.Data == null) return BadRequest(response.Message);

        return Ok(response.Data);
    }




    [HttpGet("Staffs")]
    public async Task<ActionResult<PagedResponse<Staff>>> Staffs([FromQuery] PaginationParameters paginationParameters, [FromQuery] string? onFilter, [FromQuery] string? onPosition)
    {
        var staffs = await _staffService.GetFilteredStaffAsync(paginationParameters, onFilter, onPosition);

        return Ok(staffs);
    }


    [HttpGet("StaffSummary")]
    public async Task<ActionResult<Response<List<StaffSummaryDto>>>> GetAllStaff()
    {
        var staff = await _staffService.GetAllStaffSortedByExperience();

        if (staff.Data == null)
        {
            staff.Data = new List<StaffSummaryDto>();
        }

        return Ok(staff);
    }


    [HttpGet("StaffDetails")]
    public async Task<ActionResult<StaffDetailsDto>> StaffDetails(Guid id)
    {
        var staffDetails = await _staffService.GetStaffDetailsByIdAsync(id);
        if (staffDetails.Data == null) return BadRequest(staffDetails.Message);

        return Ok(staffDetails.Data);
    }


    [Authorize(Policy = "AdminOnly")]
    [HttpPost("Create")]
    public async Task<ActionResult<string>> Create([FromBody] ExternalUserDto externalUserDto)
    {
        var response = await _staffService.AddStaffAsync(externalUserDto);
        if (response.Data == null) return BadRequest($"{response.Message}");

        return Ok(response.Message);
    }



    [Authorize(Policy = "StaffOnly")]
    [HttpPut("Update")]
    public async Task<ActionResult<string>> Update([FromBody] UpdateUserModel updateUserModel)
    {
        var result = await _staffService.UpdateStaffDataAsync(updateUserModel);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }




    [Authorize(Policy = "StaffOnly")]
    [HttpPatch("ChangePosition/{staffId}")]
    public async Task<ActionResult<string>> ChangePosition(Guid staffId, [FromBody] PositionUpdateDto positionUpdateDto)
    {
        var result = await _staffService.ChangePositionAsync(staffId, positionUpdateDto.Position);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }



    [Authorize(Policy = "AdminOnly")]
    [HttpPatch("SetIsWorkingOrDismissed/{id}")]
    public async Task<ActionResult<string>> SetIsWorkingOrDismissed(Guid id)
    {
        var result = await _staffService.SetIsWorkingOrDismissedAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }


    [Authorize(Policy = "StaffOnly")]
    [HttpPatch("SetExperience/{id}")]
    public async Task<ActionResult<string>> SetExperience(Guid id, [FromBody] ExperienceUpdateDto experienceUpdateDto)
    {
        var result = await _staffService.SetStaffExperienceAsync(id, experienceUpdateDto.Experience);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
