using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Staffs;

namespace PersonalAccount.API.Services.Abstractions;

public interface IAdminService
{
    public Task<Response<Staff>> AddAdminAsync(ExternalUserDto userModel);
}