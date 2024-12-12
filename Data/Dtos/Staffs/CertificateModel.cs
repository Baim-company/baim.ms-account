namespace PersonalAccount.API.Models.Dtos.Staffs;
public record CertificateModel
{
    public string Name { get; set; } = string.Empty;
    public string Authority { get; set; } = string.Empty;
    public string? CertificateFileStr { get; set; }
    public string Link { get; set; } = string.Empty;
    public DateTime Deadline { get; set; }
    public DateTime GivenTime { get; set; } = DateTime.UtcNow;
}