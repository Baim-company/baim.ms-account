using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Clients;

namespace PersonalAccount.API.Services.Abstractions;
public interface ITypeOfActivityService
{
    Task<List<TypeOfActivity>> GetTypesAsync();
    Task<Response<TypeOfActivity>> GetTypeAsync(Guid id); 
    Task<Response<TypeOfActivity>> AddTypeAsync(TypeOfActivityModel typeModel);
    Task<(List<TypeOfActivity>?, TypeOfActivity?)> AddTypesRangeAsync(List<TypeOfActivityModel> typeModels);
    Task<Response<TypeOfActivity>> UpdateTypeAsync(UpdateTypeOfActivityModel typeModel); 
    Task<Response<TypeOfActivity>> DeleteTypeAsync(Guid id);
}