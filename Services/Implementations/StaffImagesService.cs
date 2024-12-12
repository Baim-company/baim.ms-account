using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Staffs;
using PersonalAccount.API.Services.Abstractions;
using PersonalAccount.API.Models.Dtos.Staffs;
using PersonalAccount.API.Data.DbContexts;
using Global.Infrastructure.Exceptions.PersonalAccount;

namespace PersonalAccount.API.Services.Implementations;

public class StaffImagesService : IStaffImagesService
{
    private readonly AgileDbContext _agileDbContext;

    public StaffImagesService(AgileDbContext agileDbContext)
    {
        _agileDbContext = agileDbContext;
    }



    public async Task<Response<StaffImage>> AddImagesAsync(Guid staffId, List<ImageModel> imageModels)
    {
        try
        {
            var staff = await _agileDbContext.Staff.FindAsync(staffId);
            if (staff == null) throw new PersonalAccountException(PersonalAccountErrorType.StaffNotFound, $"Error!\nStaff with id: {staffId} doesn't exist!");

            List<StaffImage> images = new();
            foreach (var imageModel in imageModels)
            {
                var img = new StaffImage(imageModel, staff);
                images.Add(img);
            }

            await _agileDbContext.StaffImages.AddRangeAsync(images);
            await _agileDbContext.SaveChangesAsync();

            return new Response<StaffImage>("Image(s) successfully added", new StaffImage());
        }
        catch (Exception)
        {
            throw;
        }
    }




    public async Task<Response<StaffImage>> DeleteImageAsync(Guid id)
    {
        try
        {
            var img = await _agileDbContext.StaffImages.FindAsync(id);
            if (img == null) throw new PersonalAccountException(PersonalAccountErrorType.StaffNotFound, $"Error!Staff image with id: {id} doesn't exist");

            await _agileDbContext.StaffImages
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();
            await _agileDbContext.SaveChangesAsync();

            return new Response<StaffImage>("Successfully deleted", new StaffImage());
        }
        catch (Exception)
        {
            throw;
        }
    }





    public async Task<Response<StaffImage>> UpdateImagesAsync(List<UpdateImageModel> imageModels)
    {
        try
        {

            if (imageModels == null || imageModels.Count == 0)
                return new Response<StaffImage>("Error!Image models are null or empty here", null);

            foreach (var imageModel in imageModels)
            {
                var image = await _agileDbContext.StaffImages
                            .Include(s => s.Staff)
                            .Where(s => s.Id == imageModel.Id)
                            .FirstOrDefaultAsync();
                if (image == null)
                    return new Response<StaffImage>($"Error!Failed to get image with staff id: {imageModel.Id}", null);


                byte[]? imageFile = null;
                string imageFileType = "data:image/png;base64,";
                if (!string.IsNullOrEmpty(imageModel.Image))
                {
                    imageFile = Convert.FromBase64String(imageModel.Image.Split(',')[1]);
                    imageFileType = imageModel.Image.Split(',')[0] + ",";
                }


                image.IsPageImage = imageModel.IsPageImage;
                image.Image = imageFile;
                image.ImageType = imageFileType;

                _agileDbContext.StaffImages.Update(image);
                await _agileDbContext.SaveChangesAsync();
            }

            return new Response<StaffImage>("Images successfully updated", new StaffImage());
        }
        catch (Exception)
        {
            throw;
        }
    }
}