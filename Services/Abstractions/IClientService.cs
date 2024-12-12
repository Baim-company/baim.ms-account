using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Clients;

namespace PersonalAccount.API.Services.Abstractions;
public interface IClientService
{
    public Task<Response<Client>> GetClientByUserIdAsync(Guid userId);
    public Task<Response<Client>> GetClientAsync(Guid id);
    public Task<PagedResponse<Client>> GetFilteredClientsAsync(PaginationParameters paginationParameters, string? onFilter);

    public Task<Response<Client>> AddClientAdminAsync(ExternalUserDto externalUserDto);
    public Task<Response<Client>> AddClientAsync(ExternalUserDto externalUserDto, Guid companyId);
    public Task<Response<Client>> UpdateClientDataAsync(UpdateUserModel updateUserModel); 
    public Task<Response<Client>> UpdateClientActiveStatusAsync(Guid id); 
}