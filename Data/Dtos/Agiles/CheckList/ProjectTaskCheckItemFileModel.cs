namespace PersonalAccount.API.Models.Dtos.Agiles.CheckItems;

public class ProjectTaskCheckItemFileModel
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileContentBase64 { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;
}