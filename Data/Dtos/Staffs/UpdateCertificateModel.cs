namespace PersonalAccount.API.Models.Dtos.Staffs;

public record UpdateCertificateModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Authority { get; set; } = string.Empty;
    public string CertificateFilePath { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public DateTime Deadline { get; set; }
    public DateTime GivenTime { get; set; } = DateTime.UtcNow;
}