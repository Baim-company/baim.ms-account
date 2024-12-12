namespace PersonalAccount.API.Models.Entities.Agiles.CheckItems;

public class ProjectTaskCheckItemFile
{
    public Guid Id { get; set; }
    public byte[] FileData { get; set; } = Array.Empty<byte>();
    public string FileName { get; set; } = string.Empty;
    public string FileExtension { get; set; } = string.Empty;

    public Guid ProjectTaskCheckItemId { get; set; }
    public ProjectTaskCheckItem ProjectTaskCheckItem { get; set; } = null!;

    public ProjectTaskCheckItemFile()
    {
        Id = Guid.NewGuid();
    }
}