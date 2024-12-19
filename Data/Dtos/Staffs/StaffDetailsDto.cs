namespace PersonalAccount.API.Models.Dtos.Staffs;

public class StaffDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Position { get; set; }
    public ushort Experience { get; set; }
    public List<string> StaffImages { get; set; } = new List<string>();
    public int CompletedProjectsCount { get; set; }
    public int ClientsInCompletedProjectsCount { get; set; }
    public List<ProjectDetailsDto> Projects { get; set; } = new List<ProjectDetailsDto>();
    public List<CertificateDetailsDto> Сertificates { get; set; } = new List<CertificateDetailsDto>();
}