using PersonalAccount.API.Data.Dtos.FileDtos;
using PersonalAccount.API.Models.Dtos.Responses;

namespace PersonalAccount.API.Services.Abstractions;

public interface IFileService
{
    Task<Response<FileDto>> GetFileByNameAsync(string fileName);
    Task<Response<string>> UpdateFileAsync(string fileName, IFormFile newFile);
    Task<Response<string>> CreateFileAsync(IFormFile newFile);
    Response<bool> DeleteFile(string fileName);
}