using PersonalAccount.API.Data.Dtos.Staffs;
using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Dtos.Staffs;
using PersonalAccount.API.Models.Entities.Staffs;

namespace PersonalAccount.API.Services.Abstractions;
public interface IStaffService
{
    public Task<Response<Staff>> GetStaffAsync(Guid id); 
    public Task<PagedResponse<Staff>> GetFilteredStaffAsync(PaginationParameters? paginationParameters = null, string? orFilter = null,string? orPosition = null);
     
    public Task<Response<Staff>> AddStaffAsync(ExternalUserDto externalUserDto);
    public Task<Response<Staff>> UpdateStaffDataAsync(UpdateUserModel updateUserModel);

    public Task<Response<Staff>> SetStaffExperienceAsync(Guid id, ushort experience);
    public Task<Response<Staff>> SetIsWorkingOrDismissedAsync(Guid id);
    Task<Response<Staff>> ChangePositionAsync(UpdatePosition updatePosition);
    Task<Response<List<StaffSummaryDto>>> GetAllStaffSortedByExperience();
    Task<Response<StaffDetailsDto>> GetStaffDetailsByIdAsync(Guid staffId)


}