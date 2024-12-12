using PersonalAccount.API.Models.Entities.Agiles.CheckItems;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using System.Linq;

namespace PersonalAccount.API.Models.Dtos.Agiles.CheckItems
{
    public class ProjectTaskCheckListModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsImportant { get; set; }

        public Guid ProjectTaskId { get; set; }

        public IEnumerable<ProjectTaskCheckItemModel> ProjectTaskCheckItems { get; set; } = new List<ProjectTaskCheckItemModel>();

        public string ProgressDetails => $"{CompletedItemsCount}/{TotalItemsCount}";
        public int CompletedItemsCount => ProjectTaskCheckItems.Count(item => item.IsDone);
        public int TotalItemsCount => ProjectTaskCheckItems.Count();

    }
}
