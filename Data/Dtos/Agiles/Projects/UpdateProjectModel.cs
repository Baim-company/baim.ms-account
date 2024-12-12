namespace PersonalAccount.API.Models.Dtos.Agiles.Projects;
public record UpdateProjectModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Id1C { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string AvatarImage { get; set; } = string.Empty;
    public string DesignThemeImage { get; set; } = string.Empty;

    public Guid ProductId { get; set; }
    public Guid ManagerId { get; set; }

    public List<Guid>? ProjectClients { get; set; }
    public List<Guid>? ProjectStaffs { get; set; }

    public DateTime FinishDate { get; set; }
}