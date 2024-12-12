using PersonalAccount.API.Models.Dtos.Staffs;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace PersonalAccount.API.Models.Entities.Staffs;
public class StaffImage
{
    public Guid Id { get; set; }
    public bool IsPageImage { get; set; }


    public string CombinedImage => Image != null && Image.Length > 0 ? $"{ImageType}{Convert.ToBase64String(Image)}" : ImageType;
    [JsonIgnore]
    public byte[]? Image { get; set; } = Array.Empty<byte>();
    [JsonIgnore]
    public string ImageType { get; set; } = "data:image/png;base64,";

    [JsonIgnore]
    public Guid StaffId { get; set; }
    [JsonIgnore]
    public Staff Staff { get; set; }


    public StaffImage()
    {
        Id = Guid.NewGuid();
    }
    public StaffImage(ImageModel imageModel, Staff staff)
    {
        Id = Guid.NewGuid();
        StaffId = staff.Id;
        Staff = staff;
        IsPageImage = imageModel.IsPageImage;

        if (!string.IsNullOrWhiteSpace(imageModel.Image))
        {
            Image = Convert.FromBase64String(imageModel.Image.Split(',')[1]);
            ImageType = imageModel.Image.Split(',')[0] + ",";
        }
    } 
}
