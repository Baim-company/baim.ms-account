using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Dtos.Staffs;
using PersonalAccount.API.Models.Entities.Staffs;

namespace PersonalAccount.API.Services.Abstractions;
public interface IStaffImagesService
{ 
    public Task<Response<StaffImage>> AddImagesAsync(Guid staffId, List<ImageModel> imageModels);
    public Task<Response<StaffImage>> UpdateImagesAsync(List<UpdateImageModel> imageModels);
    public Task<Response<StaffImage>> DeleteImageAsync(Guid id);
}