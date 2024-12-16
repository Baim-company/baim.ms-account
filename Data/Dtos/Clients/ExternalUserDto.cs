using PersonalAccount.API.Models.Enums;


namespace PersonalAccount.API.Models.Dtos.Clients;

public class ExternalUserDto
{
    public Guid Id { get; set; }
    public string? Position { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Role { get; set; } = "User";

    public string Email { get; set; } = string.Empty;
    public string PersonalEmail { get; set; } = string.Empty;



    public string PhoneNumber { get; set; } = string.Empty;
    public string BusinessPhoneNumber { get; set; } = string.Empty;


    public Gender Gender { get; set; } = Gender.Man;
    public string AvatarPath { get; set; } = "";
}