using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Clients;


namespace PersonalAccount.API.Services.Abstractions;

public interface ITypeOfActivityService
{
    Task<List<string>> GetTypesAsync();
    Task<Response<TypeOfActivity>> GetTypeAsync(Guid id); 
    Task<Response<TypeOfActivity>> AddTypeAsync(string title);
    Task<Response<List<TypeOfActivity>>> AddTypesRangeAsync(List<string> titles);
    Task<Response<TypeOfActivity>> UpdateTypeAsync(Guid id, string title); 
    Task<Response<TypeOfActivity>> DeleteTypeAsync(Guid id);
}