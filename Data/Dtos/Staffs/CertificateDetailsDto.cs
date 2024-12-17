namespace PersonalAccount.API.Models.Dtos.Staffs;

public record CertificateDetailsDto
{
    public string Name { get; set; }
    public string Authority { get; set; }
    public DateTime GivenTime { get; set; }
    public DateTime Deadline { get; set; }
    public string Link { get; set; }
    public string CertificateFilePath { get; set; }
}