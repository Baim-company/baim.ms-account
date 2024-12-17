using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Services.Abstractions;


namespace PersonalAccount.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Company>> Company(Guid id)
    {
        var response = await _companyService.GetCompanyByIdAsync(id);

        if (response == null) return NotFound($"Error!Failed to get company with id: {id}!");

        return Ok(response);
    }



    [HttpGet("Companies")]
    public async Task<ActionResult<PagedResponse<Company>>> Companies([FromQuery] PaginationParameters paginationParameters,
        [FromQuery] string? onSearch = null,
        [FromQuery] string? onFilterProduct = null,
        [FromQuery] string? onFilterTypeOfActivity = null,
        [FromQuery] string? onFilterNational = null)
    {
        var companies = await _companyService.GetFilteredCompaniesAsync(paginationParameters, onSearch, onFilterProduct, onFilterTypeOfActivity, onFilterNational);

        return Ok(companies);
    }



    [Authorize(Policy = "UserAdminOnly")]
    [HttpPut("CompleteCompany")]
    public async Task<ActionResult<string>> CompleteCompany([FromBody] CompanyModel company)
    {
        var result = await _companyService.CompleteCompanyAsync(company);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }




    [Authorize(Policy = "UserAdminOnly")]
    [HttpPut("Update")]
    public async Task<ActionResult<string>> Update([FromBody] UpdateCompanyModel updateCompany)
    {
        var result = await _companyService.UpdateCompanyDataAsync(updateCompany);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }



    [Authorize(Policy = "HighPriorityOnly")]
    [HttpPatch("Companies/{id}/ToggleNationalStatus")]
    public async Task<ActionResult<string>> ToggleNationalStatus(Guid id)
    {
        var result = await _companyService.SetIsNationalOrPrivateAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }


    [Authorize(Policy = "AdminAndStaffOnly")]
    [HttpPatch("Companies/{id}/TogglePublicStatus")]
    public async Task<ActionResult<string>> TogglePublicStatus(Guid id)
    {
        var result = await _companyService.ChangeIsPublicAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}