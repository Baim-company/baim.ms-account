namespace PersonalAccount.API.Models.Dtos.Staffs;

public record UpdatePosition
{
    public Guid Id { get; set; }
    public string Position { get; set; } = string.Empty;
}