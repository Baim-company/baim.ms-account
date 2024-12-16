using PersonalAccount.API.Data.Dtos.FileDtos;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Services.Implementations;

public class FileService : IFileService
{
    private readonly string _imagesRootPath;

    public FileService(IConfiguration configuration)
    {
        _imagesRootPath = configuration["ImagesRootPath"]
            ?? throw new Exception("ImagesRootPath is not configured");
    }


    public async Task<Response<FileDto>> GetFileByNameAsync(string fileName)
    {
        var filePath = Path.Combine(
            _imagesRootPath,
            fileName);
        if (!File.Exists(filePath))
            return new Response<FileDto>($"File not found at path: {filePath}");

        var fileBytes = await File.ReadAllBytesAsync(filePath);
        var contentType = GetContentType(filePath);

        return new Response<FileDto>("Success", new FileDto
        {
            FileName = fileName,
            ContentType = contentType,
            FileContent = fileBytes
        });
    }


    public async Task<Response<string>> CreateFileAsync(IFormFile newFile)
    {
        if (newFile == null || newFile.Length == 0)
            return new Response<string>("New file were provided.");

        var fileExtension = Path.GetExtension(newFile.FileName);

        var fileName = $"{Guid.NewGuid()}{fileExtension}";

        var filePath = Path.Combine(
            _imagesRootPath,
            fileName
        );

        await using var stream = File.Create(filePath);
        await newFile.CopyToAsync(stream);

        return new Response<string>("File was uploaded successfully.", fileName);
    }


    public async Task<Response<string>> UpdateFileAsync(string fileName, IFormFile newFile)
    {
        if (newFile == null || newFile.Length == 0)
            return new Response<string>("New file is empty or not provided.");

        var oldFilePath = Path.Combine(_imagesRootPath, fileName);


        var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(newFile.FileName)}";
        var newFilePath = Path.Combine(_imagesRootPath, newFileName);

        try
        {
            await using var stream = File.Create(newFilePath);
            await newFile.CopyToAsync(stream);
        }
        catch (Exception ex)
        {
            return new Response<string>($"Failed to save the new file: {ex.Message}");
        }


        if (File.Exists(oldFilePath))
        {
            try
            {
                File.Delete(oldFilePath);
            }
            catch (Exception ex)
            {
                return new Response<string>($"Failed to delete the old file: {ex.Message}");
            }
        }

        return new Response<string>("File updated successfully!", newFileName);
    }


    public Response<bool> DeleteFile(string fileName)
    {
        var filePath = Path.Combine(_imagesRootPath, 
            fileName);
        if (!File.Exists(filePath))
            return new Response<bool>("Error: File not found or directory does not exist.", false);

        File.Delete(filePath);

        return new Response<bool>("File was successfully deleted.", true);
    }






    private string GetContentType(string path)
    {
        var types = new Dictionary<string, string>
        {
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".bmp", "image/bmp" },
            { ".svg", "image/svg+xml" }
        };

        return types.TryGetValue(Path.GetExtension(path).ToLowerInvariant(), out var contentType)
            ? contentType
            : "application/octet-stream";
    }
}