namespace PersonalAccount.API.Models.Dtos.Agiles.CheckItems;

public class ProjectTaskCheckItemModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDone { get; set; }
    public bool IsImportant { get; set; }
    public IEnumerable<ProjectTaskCheckItemFileModel> ProjectTaskCheckItemFiles { get; set; } = new List<ProjectTaskCheckItemFileModel>();

}