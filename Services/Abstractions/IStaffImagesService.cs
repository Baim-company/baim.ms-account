using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Dtos.Staffs;
using PersonalAccount.API.Models.Entities.Staffs;

namespace PersonalAccount.API.Services.Abstractions;
public interface IStaffImagesService
{
    Task<List<StaffImage>> GetAll();
    Task<Response<StaffImage>> AddImagesAsync(Guid staffId, List<IFormFile> files);

    Task<Response<List<StaffImage>>> UpdateImagesAsync(Guid staffId, List<IFormFile> newFiles);
    Task<Response<StaffImage>> UpdateImageAsync(string lastFileName, IFormFile newFile);

    Task<Response<StaffImage>> DeleteImageAsync(Guid id);
}