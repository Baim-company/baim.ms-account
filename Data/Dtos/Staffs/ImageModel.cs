namespace PersonalAccount.API.Models.Dtos.Staffs;
public record ImageModel
{
    public bool IsPageImage { get; set; }
    public string Image { get; set; } = string.Empty;
}