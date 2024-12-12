using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Clients;

namespace PersonalAccount.API.Services.Abstractions;
public interface ICompanyService
{ 
    public Task<PagedResponse<Company>> GetFilteredCompaniesAsync(
        PaginationParameters paginationParameters,
        string? onSearch = null,
        string? onFilterProduct = null,
        string? onFilterTypeOfActivity = null,
        string? onFilterNational = null);
    Task<Company?> GetCompanyByIdAsync(Guid id);
    Task<Response<Company>> CompleteCompanyAsync(CompanyModel companyModel);
    Task<Response<Company>> UpdateCompanyDataAsync(UpdateCompanyModel companyModel);
    Task<Response<Company>> SetIsNationalOrPrivateAsync(Guid id);
    Task<Response<Company>> ChangeIsPublicAsync(Guid id);
}