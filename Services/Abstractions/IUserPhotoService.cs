using PersonalAccount.API.Data.Dtos.FileDtos;
using PersonalAccount.API.Models.Dtos.Responses;

namespace PersonalAccount.API.Services.Abstractions;

public interface IUserPhotoService
{
    Task<Response<FileDto>> GetFileByUserIdAsync(Guid userId);

    Task<Response<bool>> UpdateFilePathAsync(Guid userId, string filePath);
    Task<Response<bool>> DeleteFileAsync(Guid userId, string filePath);
}