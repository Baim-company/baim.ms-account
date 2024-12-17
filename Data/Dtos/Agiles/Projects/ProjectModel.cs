using PersonalAccount.API.Models.Enums;

namespace PersonalAccount.API.Models.Dtos.Agiles.Projects;
public record ProjectModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;


    public string ProjectAvatarImagePath { get; set; } = string.Empty;
    public string DesignThemeImagePath { get; set; } = string.Empty;


    public Guid ProductId { get; set; }
    public Guid ManagerId { get; set; }
    public Guid CompanyId { get; set; }


    public List<Guid>? ProjectClients { get; set; }
    public List<Guid>? ProjectStaffs { get; set; }


    public DateTime FinishDate { get; set; }
    public ProjectType ProjectType { get; set; } = ProjectType.External;
}