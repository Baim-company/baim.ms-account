using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Dtos.Staffs;
using PersonalAccount.API.Models.Entities.Staffs;

namespace PersonalAccount.API.Services.Abstractions;
public interface ICertificateService
{ 
    public Task<Response<Certificate>> AddCertificatesAsync(Guid staffId, List<CertificateModel> certificateModels);
    public Task<Response<Certificate>> UpdateCertificatesAsync(List<UpdateCertificateModel> certificateModels);
    public Task<List<Certificate>> GetCertificatesAsync(Guid staffId);
    Task<Certificate> GetCertificateAsync(Guid id);
    public Task<Response<Certificate>> DeleteCertificatesAsync(Guid id);
}