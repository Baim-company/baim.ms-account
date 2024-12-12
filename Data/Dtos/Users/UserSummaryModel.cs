using PersonalAccount.API.Models.Entities.Users;
using PersonalAccount.API.Models.Enums;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Dtos.Users;

public class UserSummaryModel
{
    public Guid Id { get; set; }
    public string? Id1C { get; set; }
    public string? Position { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Patronimyc { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Email { get; set; }
    public string? PersonalEmail { get; set; }
    public bool EmailConfirmed { get; set; }

    public string? PhoneNumber { get; set; }
    public string? BusinessPhoneNumber { get; set; }



    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender { get; set; } = Gender.Man;


    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();


    [JsonIgnore]
    public byte[] Image = Array.Empty<byte>();
    [JsonIgnore]
    public string ImageType = "data:image/png;base64,";
    public string CombinedImage => Image.Length > 0 ? $"{ImageType}{Convert.ToBase64String(Image)}" : ImageType;

}
