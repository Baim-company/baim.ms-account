using PersonalAccount.API.Data.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Clients;


namespace PersonalAccount.API.Services.Abstractions;

public interface ITypeOfActivityService
{
    Task<List<TypeOfActivityDto>> GetTypesAsync();
    Task<Response<TypeOfActivityDto>> GetTypeAsync(Guid id); 
    Task<Response<TypeOfActivityDto>> AddTypeAsync(TypeOfActivityDto typeOfActivityDto);
    Task<Response<List<TypeOfActivityDto>>> AddTypesRangeAsync(List<TypeOfActivityDto> typeOfActivityDtos);
    Task<Response<TypeOfActivityDto>> UpdateTypeAsync(Guid id, TypeOfActivityDto typeOfActivityDto); 
    Task<Response<TypeOfActivityDto>> DeleteTypeAsync(Guid id);
}