using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Staffs;

public class StaffImage
{
    public Guid Id { get; set; }
    public string ImagePath { get; set; }


    [JsonIgnore]
    public Guid StaffId { get; set; }
    [JsonIgnore]
    public Staff Staff { get; set; }


    public StaffImage()
    {
        Id = Guid.NewGuid();
    }
    public StaffImage(string imagePath, Staff staff)
    {
        Id = Guid.NewGuid();
        StaffId = staff.Id;
        Staff = staff;

        ImagePath = imagePath;
    } 
}
