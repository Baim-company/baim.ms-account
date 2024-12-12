using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Clients;
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
    public async Task<IActionResult> Company(Guid id)
    {
        var response = await _companyService.GetCompanyByIdAsync(id);

        if (response == null) return BadRequest($"Error!Failed to get company with id: {id}!");

        return Ok(response);
    }



    [HttpGet("Companies")]
    public async Task<IActionResult> Companies([FromQuery] PaginationParameters paginationParameters,
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
    public async Task<IActionResult> CompleteCompany([FromBody] CompanyModel company)
    {
        var result = await _companyService.CompleteCompanyAsync(company);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Data);
    }




    [Authorize(Policy = "UserAdminOnly")]
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateCompanyModel updateCompany)
    {
        var result = await _companyService.UpdateCompanyDataAsync(updateCompany);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Data);
    }



    [Authorize(Policy = "UserAdminOnly")] 
    [HttpPut("SetIsNationalOrPrivate/{id}")]
    public async Task<IActionResult> SetIsNationalOrPrivate(Guid id)
    {
        var result = await _companyService.SetIsNationalOrPrivateAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Data);
    }



    [Authorize(Policy = "AdminOnly")]
    [HttpPut("ChangeIsPublic/{id}")]
    public async Task<IActionResult> ChangeIsPublic(Guid id)
    {
        var result = await _companyService.ChangeIsPublicAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Data);
    }
}