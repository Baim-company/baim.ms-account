using PersonalAccount.API.Models.Entities.Staffs;
using PersonalAccount.API.Services.Abstractions;
using PersonalAccount.API.Models.Dtos.Staffs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace PersonalAccount.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CertificateController : ControllerBase
{
    private readonly ICertificateService _certificateService;

    public CertificateController(ICertificateService certificateService)
    {
        _certificateService = certificateService;
    }


    [HttpGet("Certificates/Staff/{staffId}")]
    public async Task<ActionResult<List<Certificate>>> Certificates(Guid staffId)
    {
        var response = await _certificateService.GetCertificatesAsync(staffId);

        return Ok(response);
    }

    [HttpGet("Certificate/{id}")]
    public async Task<ActionResult<Certificate>> Certificate(Guid id)
    {
        var response = await _certificateService.GetCertificateAsync(id);

        return Ok(response);
    }


    [Authorize(Policy = "StaffOnly")]
    [HttpPost("Create")]
    public async Task<ActionResult<string>> Add([FromHeader]Guid staffId, [FromBody] List<CertificateModel> certificateModels)
    {
        var result = await _certificateService.AddCertificatesAsync(staffId, certificateModels);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }


    [Authorize(Policy = "StaffOnly")]
    [HttpPut("Update")]
    public async Task<ActionResult<string>> Update([FromBody] List<UpdateCertificateModel> certificateModels)
    {
        var result = await _certificateService.UpdateCertificatesAsync(certificateModels);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }



    [Authorize(Policy = "StaffOnly")]
    [HttpDelete("Delete")]
    public async Task<ActionResult<string>> Delete([FromHeader] Guid id)
    {
        var result = await _certificateService.DeleteCertificatesAsync(id);
        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}