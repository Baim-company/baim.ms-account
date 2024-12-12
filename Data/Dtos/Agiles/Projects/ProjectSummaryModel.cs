using PersonalAccount.API.Models.Entities.Agiles.Projects;
using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Models.Entities.Staffs;
using PersonalAccount.API.Models.Enums;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Dtos.Agiles.Projects
{
    public class ProjectSummaryModel
    {
        public Guid Id { get; set; }
        public string? Id1C { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsPublic { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProjectType ProjectType { get; set; } = ProjectType.External;
        public Product Product { get; set; }
        public Staff ProjectManager { get; set; }

        [JsonIgnore]
        public byte[]? DesignTheme { get; set; }
        public string DesignThemeImageType { get; set; } = "data:image/jpeg;base64,";
        public string DesignThemeCombinedImage => $"{DesignThemeImageType}{Convert.ToBase64String(DesignTheme!)}";

        [JsonIgnore]
        public byte[]? Avatar { get; set; }
        public string AvatarImageType { get; set; } = "data:image/png;base64,";
        public string AvatarCombinedImage => $"{AvatarImageType}{Convert.ToBase64String(Avatar!)}";
        public Company? Company { get; set; }
        public List<ProjectUser> ProjectUsers { get; set; }


    }
}
