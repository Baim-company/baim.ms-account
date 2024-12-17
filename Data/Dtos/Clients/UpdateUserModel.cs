using PersonalAccount.API.Models.Enums;

namespace PersonalAccount.API.Models.Dtos.Clients;
public record UpdateUserModel
{
    public Guid Id { get; set; }
    public string? Position { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; } = DateTime.MinValue;

    public string Email { get; set; } = string.Empty;
    public string PersonalEmail { get; set; } = string.Empty;



    public string PhoneNumber { get; set; } = string.Empty;
    public string BusinessPhoneNumber { get; set; } = string.Empty;


    public Gender Gender { get; set; } = Gender.Man;


    public UpdateUserModel() { }
    public UpdateUserModel(ExternalUserDto externalUserDto)
    {
        Id = externalUserDto.Id;
        Position = externalUserDto.Position ?? "";
        Name = externalUserDto.Name;
        Surname = externalUserDto.Surname ?? "";
        Patronymic = externalUserDto.Patronymic ?? "";
        BirthDate = externalUserDto.BirthDate;

        Email = externalUserDto.Email;
        PersonalEmail = externalUserDto.PersonalEmail ?? "";

        PhoneNumber = externalUserDto.PhoneNumber ?? "";
        BusinessPhoneNumber = externalUserDto.BusinessPhoneNumber ?? "";

        Gender = externalUserDto.Gender;
    }
}