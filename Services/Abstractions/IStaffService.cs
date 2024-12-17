using PersonalAccount.API.Data.Dtos.Staffs;
using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Dtos.Staffs;
using PersonalAccount.API.Models.Entities.Staffs;

namespace PersonalAccount.API.Services.Abstractions;
public interface IStaffService
{
    Task<Response<Staff>> GetStaffAsync(Guid id); 
    Task<PagedResponse<Staff>> GetFilteredStaffAsync(PaginationParameters? paginationParameters = null, string? orFilter = null,string? orPosition = null);
    
    Task<Response<Staff>> AddStaffAsync(ExternalUserDto externalUserDto);
    Task<Response<Staff>> UpdateStaffDataAsync(UpdateUserModel updateUserModel);

    Task<Response<Staff>> SetStaffExperienceAsync(Guid id, ushort experience);
    Task<Response<Staff>> SetIsWorkingOrDismissedAsync(Guid id);
    Task<Response<Staff>> ChangePositionAsync(Guid id, string position);
    Task<Response<List<StaffSummaryDto>>> GetAllStaffSortedByExperience();
    Task<Response<StaffDetailsDto>> GetStaffDetailsByIdAsync(Guid staffId);

}