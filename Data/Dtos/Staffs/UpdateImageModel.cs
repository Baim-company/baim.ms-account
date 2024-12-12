namespace PersonalAccount.API.Models.Dtos.Staffs;
public record UpdateImageModel
{
    public Guid Id { get; set; }
    public bool IsPageImage { get; set; }
    public string? Image { get; set; } = string.Empty;
}