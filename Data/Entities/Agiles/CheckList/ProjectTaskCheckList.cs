using PersonalAccount.API.Models.Entities.Agiles.CheckItems;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;

namespace PersonalAccount.API.Models.Entities.Agiles.CheckLists;

public class ProjectTaskCheckList
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(4);
    public bool IsImportant { get; set; }

    public Guid ProjectTaskId { get; set; }
    public ProjectTask ProjectTask { get; set; } = null!;

    public ICollection<ProjectTaskCheckItem> ProjectTaskCheckItems { get; set; } = new List<ProjectTaskCheckItem>();

    public string ProgressDetails => $"{CompletedItemsCount}/{TotalItemsCount}";
    public int CompletedItemsCount => ProjectTaskCheckItems.Count(item => item.IsDone);
    public int TotalItemsCount => ProjectTaskCheckItems.Count;
    public ProjectTaskCheckList()
    {
        ProjectTaskCheckItems = new HashSet<ProjectTaskCheckItem>();
    }
    public ProjectTaskCheckList(ProjectTask projectTask, string title)
    {
        Id = Guid.NewGuid();
        Title = title;
        ProjectTask = projectTask ?? throw new ArgumentNullException(nameof(projectTask));
        ProjectTaskId = projectTask.Id;
        CreatedDate = DateTime.UtcNow.AddHours(4);
    }

    public void AddCheckItem(ProjectTaskCheckItem checkItem)
    {
        if (checkItem == null)
            throw new ArgumentNullException(nameof(checkItem));

        ProjectTaskCheckItems.Add(checkItem);
    }
}