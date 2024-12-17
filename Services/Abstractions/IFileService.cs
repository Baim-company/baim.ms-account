using PersonalAccount.API.Data.Dtos.FileDtos;
using PersonalAccount.API.Models.Dtos.Responses;

namespace PersonalAccount.API.Services.Abstractions;

public interface IFileService
{
    Task<Response<FileDto>> GetFileByNameAsync(string fileName);

    Task<Response<string>> CreateFileAsync(IFormFile newFile);
    Task<Response<List<string>>> CreateFilesAsync(List<IFormFile> files);

    Task<Response<string>> UpdateFileAsync(string fileName, IFormFile newFile);
    Task<Response<List<string>>> UpdateFilesAsync(List<(string oldFileName, IFormFile newFile)> fileUpdates);

    Response<bool> DeleteFile(string fileName);
}