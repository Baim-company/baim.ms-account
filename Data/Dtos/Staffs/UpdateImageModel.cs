namespace PersonalAccount.API.Models.Dtos.Staffs;
public record UpdateImageModel
{
    public Guid Id { get; set; }
    public string ImagePath { get; set; } = string.Empty;
}