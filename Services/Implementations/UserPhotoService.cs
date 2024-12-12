using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Data.Dtos.FileDtos;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Services.Implementations;

public class UserPhotoService : IUserPhotoService
{

    private readonly string _imagesRootPath;
    private readonly AgileDbContext _agileDbContext;

    public UserPhotoService(AgileDbContext agileDbContext,
        IConfiguration configuration)
    {
        _agileDbContext = agileDbContext;
        _imagesRootPath = configuration["ImagesRootPath"]
            ?? throw new Exception("ImagesRootPath is not configured");
    }


    


    public async Task<Response<FileDto>> GetFileByUserIdAsync(Guid userId)
    {
        var user = await _agileDbContext.Users.FindAsync(userId);
        if (user == null)
            return new Response<FileDto>($"File with user id: {userId} not found.");

        var filePath = Path.Combine(_imagesRootPath, user.AvatarPath);
        if (!File.Exists(filePath))
            return new Response<FileDto>($"File not found at path: {filePath}");

        var fileBytes = await File.ReadAllBytesAsync(filePath);
        var contentType = GetContentType(filePath);

        return new Response<FileDto>("Success", new FileDto
        {
            FileName = user.AvatarPath,
            ContentType = contentType,
            FileContent = fileBytes
        });
    }





    public async Task<Response<bool>> UpdateFilePathAsync(Guid userId, string filePath)
    {
        if (filePath == null || filePath.Length == 0)
            return new Response<bool>("New file path is empty or null.",false);

        var user = await _agileDbContext.Users.FindAsync(userId);
        if (user == null)
            return new Response<bool>($"User with id: {userId} not found.",false);

        user.AvatarPath = filePath;

        await _agileDbContext.SaveChangesAsync();

        return new Response<bool>("File updated successfully!", true);
    }





    public async Task<Response<bool>> DeleteFileAsync(Guid userId, string filePath)
    {
        if (filePath == null || filePath.Length == 0)
            return new Response<bool>("New file path is empty or null.");

        var user = await _agileDbContext.Users.FindAsync(userId);
        if (user == null)
            return new Response<bool>("User not found.");


        user.AvatarPath = filePath;
        await _agileDbContext.SaveChangesAsync();

        return new Response<bool>("File deleted successfully, default image set.", true);
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