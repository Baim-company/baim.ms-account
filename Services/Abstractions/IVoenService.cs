using PersonalAccount.API.Models.Dtos.Clients;

namespace PersonalAccount.API.Services.Abstractions;
public interface IVoenService
{
    Task<VoenModel> GetVoenDetailsAsync(string voen);
}