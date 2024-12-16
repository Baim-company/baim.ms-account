using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Entities.Agiles.Comments;
using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Models.Entities.Staffs;
using PersonalAccount.API.Models.Enums;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Users;

public class User
{
    public Guid Id { get; set; }
    public string? Position { get; set; }

    public string Name { get; set; }
    public string? Surname { get; set; }
    public string? Patronymic { get; set; }
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
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public string AvatarPath { get; set; }


    [JsonIgnore]
    public Staff? Staff { get; set; }
    [JsonIgnore]
    public Guid StaffId { get; set; }


    [JsonIgnore]
    public Client? Client { get; set; }
    [JsonIgnore]
    public Guid ClientId { get; set; }


    public User()
    {
        Id = Guid.NewGuid();
        Name = "";
        Email = "";
    }
    public User(ExternalUserDto externalUserDto)
    {
        Id = externalUserDto.Id;
        Name = externalUserDto.Name;
        Surname = externalUserDto.Surname;
        Patronymic = externalUserDto.Patronymic;
        BirthDate = externalUserDto.BirthDate;
        Gender = externalUserDto.Gender;

        EmailConfirmed = true;
        Email = externalUserDto.Email;
        PersonalEmail = externalUserDto.PersonalEmail;

        PhoneNumber = externalUserDto.PhoneNumber;
        BusinessPhoneNumber = externalUserDto.BusinessPhoneNumber;

        Position = externalUserDto.Position;
        AvatarPath = externalUserDto.AvatarPath;
    }
}