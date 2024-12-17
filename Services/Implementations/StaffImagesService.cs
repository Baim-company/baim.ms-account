using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Staffs;
using PersonalAccount.API.Services.Abstractions;
using PersonalAccount.API.Data.DbContexts;
using Global.Infrastructure.Exceptions.PersonalAccount;
using PersonalAccount.API.Models.Dtos.Staffs;

namespace PersonalAccount.API.Services.Implementations;

public class StaffImagesService : IStaffImagesService
{
    private readonly string _baseImageUrl;
    private readonly IFileService _fileService;
    private readonly AgileDbContext _agileDbContext;

    public StaffImagesService(AgileDbContext agileDbContext,
        IConfiguration configuration,
        IFileService fileService)
    {
        _agileDbContext = agileDbContext;
        _fileService = fileService;
        _baseImageUrl = configuration["BaseImageUrl"]
            ?? throw new Exception("BaseImageUrl is not configured");
    }



    public async Task<Response<StaffImage>> AddImagesAsync(Guid staffId, List<IFormFile> files)
    {
        var staff = await _agileDbContext.Staff.FindAsync(staffId);
        if (staff == null) throw new PersonalAccountException(PersonalAccountErrorType.StaffNotFound, $"Error!\nStaff with id: {staffId} doesn't exist!");


        var result = await _fileService.CreateFilesAsync(files);
        if (result.Data == null) return new Response<StaffImage>(result.Message);

        List<string> imagesPaths = result.Data;


        List<StaffImage> images = new();
        foreach (var imagePath in imagesPaths)
        {
            var img = new StaffImage(imagePath, staff);
            images.Add(img);
        }

        await _agileDbContext.StaffImages.AddRangeAsync(images);
        await _agileDbContext.SaveChangesAsync();

        return new Response<StaffImage>("Image(s) successfully added", new StaffImage());
    }





    public async Task<Response<List<StaffImage>>> UpdateImagesAsync(Guid staffId, List<IFormFile> newFiles)
    {
        var staffImages = await _agileDbContext.StaffImages
            .Include(s => s.Staff)
            .Where(s => s.StaffId == staffId)
            .ToListAsync();

        if (staffImages == null || !staffImages.Any())
            return new Response<List<StaffImage>>("No existing images found for the provided staff ID.");

        if (newFiles == null || !newFiles.Any())
            return new Response<List<StaffImage>>("No new files provided for update.");

        var filesToUpdate = new List<(string oldFileName, IFormFile newFile)>();

        for (int i = 0; i < Math.Min(staffImages.Count, newFiles.Count); i++)
        {
            var oldFileName = staffImages[i].ImagePath; 
            var newFile = newFiles[i];

            filesToUpdate.Add((oldFileName, newFile));
        }

        var updateResult = await _fileService.UpdateFilesAsync(filesToUpdate);

        if (updateResult.Data == null)
            return new Response<List<StaffImage>>(updateResult.Message);

        for (int i = 0; i < filesToUpdate.Count; i++)
        {
            staffImages[i].ImagePath = updateResult.Data[i];
        }

        await _agileDbContext.SaveChangesAsync();

        return new Response<List<StaffImage>>("Images successfully updated", staffImages);
    }


    // Стоит ли удалять ??? Или пусть всегда будет только PUT
    public async Task<Response<StaffImage>> DeleteImageAsync(Guid id)
    {
        var img = await _agileDbContext.StaffImages.FindAsync(id);
        if (img == null) throw new PersonalAccountException(PersonalAccountErrorType.StaffNotFound, $"Error!Staff image with id: {id} doesn't exist");


        var result = _fileService.DeleteFile(img.ImagePath);
        if (!result.Data) return new Response<StaffImage>(result.Message);


        await _agileDbContext.StaffImages
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync();
        await _agileDbContext.SaveChangesAsync();

        return new Response<StaffImage>("Successfully deleted", new StaffImage());
    }


    public async Task<Response<StaffImage>> UpdateImageAsync(string lastFileName, IFormFile newFile)
    {
        var staffImage = await _agileDbContext.StaffImages.FirstOrDefaultAsync(img => img.ImagePath == lastFileName);
        if (staffImage == null) throw new PersonalAccountException(PersonalAccountErrorType.StaffNotFound, 
            $"Error!Staff image with file name: {lastFileName} doesn't exist");

        var result = await _fileService.UpdateFileAsync(lastFileName, newFile);
        if (result.Data == null) return new Response<StaffImage>(result.Message);

        staffImage.ImagePath = result.Data;

        await _agileDbContext.SaveChangesAsync();

        return new Response<StaffImage>("Images successfully updated", staffImage);
    }


    public async Task<Response<StaffImage>> UpdateIsPageImageAsync(Guid staffImageId)
    {
        var staffImage = await _agileDbContext.StaffImages.FindAsync(staffImageId);

        if (staffImage == null)
            throw new PersonalAccountException(PersonalAccountErrorType.StaffNotFound,
                $"Error! Staff image with staff image id: {staffImageId} doesn't exist");

        await _agileDbContext.StaffImages
            .Where(si => si.Id == staffImageId)
            .ExecuteUpdateAsync(si => si
                .SetProperty(si => si.IsPageImage, si => !si.IsPageImage));

        var updatedStaffImage = await _agileDbContext.StaffImages.FindAsync(staffImageId);

        return new Response<StaffImage>("IsPageImage successfully updated.", updatedStaffImage);
    }
}