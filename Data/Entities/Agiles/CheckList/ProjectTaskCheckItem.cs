using PersonalAccount.API.Models.Entities.Agiles.CheckLists;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Agiles.CheckItems;

public class ProjectTaskCheckItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDone { get; set; }
    public bool IsImportant { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(4);


    [JsonIgnore]
    public Guid CheckListId { get; set; }
    public ProjectTaskCheckList CheckList { get; set; }


    public ICollection<ProjectTaskCheckItemFile> ProjectTaskCheckItemFiles { get; set; }
    public ProjectTaskCheckItem()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.UtcNow.AddHours(4);
    }

    public ProjectTaskCheckItem(string name, ProjectTaskCheckList checkList)
    {
        Id = Guid.NewGuid();
        Name = name;
        IsDone = false;
        CheckList = checkList;
        CheckListId = checkList.Id;
        CreatedDate = DateTime.UtcNow.AddHours(4);
    }
}
