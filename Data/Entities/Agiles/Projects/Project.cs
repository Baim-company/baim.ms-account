using PersonalAccount.API.Models.Dtos.Agiles.Projects;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Entities.Agiles.Tickets;
using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Models.Entities.Staffs;
using PersonalAccount.API.Models.Enums;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Agiles.Projects;
public class Project
{
    public Guid Id { get; set; }
    public string? Id1C { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(4);
    public DateTime FinishDate { get; set; } = DateTime.UtcNow.AddHours(4);
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsPublic { get; set; }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProjectType ProjectType { get; set; } = ProjectType.External;



    [JsonIgnore]
    public Guid ProductId { get; set; }
    public Product Product { get; set; }


    [JsonIgnore]
    public Guid ProjectManagerId { get; set; }
    public Staff ProjectManager { get; set; }



    [JsonIgnore]
    public byte[]? DesignTheme { get; set; }
    public string DesignThemeImageType { get; set; } = "data:image/jpeg;base64,";
    public string DesignThemeCombinedImage => $"{DesignThemeImageType}{Convert.ToBase64String(DesignTheme!)}";


    [JsonIgnore]
    public byte[]? Avatar { get; set; }
    public string AvatarImageType { get; set; } = "data:image/png;base64,";
    public string AvatarCombinedImage => $"{AvatarImageType}{Convert.ToBase64String(Avatar!)}";




    public Guid? CompanyId { get; set; }
    [JsonIgnore]
    public Company? Company { get; set; }

    [JsonIgnore]

    public List<ProjectTiket>? ProjectTikets { get; set; }
    [JsonIgnore]

    public ICollection<ProjectTask>? ProjectTasks { get; set; }


    // Участники Айхан Диана Кямал Фидан Медина 
    public List<ProjectUser> ProjectUsers { get; set; }


    public Project()
    {
        Id = Guid.NewGuid();
    }
    public Project(Product product, Staff manager)
    {
        Id = Guid.NewGuid();

        ProductId = product.Id;
        Product = product;

        ProjectManagerId = manager.Id;
        ProjectManager = manager;

        CreatedDate = DateTime.UtcNow.AddHours(4);
    }
    public Project(ProjectModel projectModel)
    {
        Id = Guid.NewGuid();

        Id1C = projectModel.Id1C;
        Name = projectModel.Name;
        Description = projectModel.Description;

        ProductId = projectModel.ProductId;
        ProjectManagerId = projectModel.ManagerId;

        CreatedDate = DateTime.UtcNow.AddHours(4);
        FinishDate = projectModel.FinishDate;
        ProjectType = projectModel.ProjectType;

        if (!string.IsNullOrEmpty(projectModel.AvatarImage))
        {
            Avatar = Convert.FromBase64String(projectModel.AvatarImage.Split(',')[1]);
            AvatarImageType = projectModel.AvatarImage.Split(',')[0] + ",";
        }
        if (!string.IsNullOrEmpty(projectModel.DesignThemeImage))
        {
            DesignTheme = Convert.FromBase64String(projectModel.DesignThemeImage.Split(',')[1]);
            DesignThemeImageType = projectModel.DesignThemeImage.Split(',')[0] + ",";
        }

        if (projectModel.ProjectType == ProjectType.External)
        {
            CompanyId = projectModel.CompanyId;
        }
        else projectModel.ProjectClients = null;


        if ((projectModel.ProjectStaffs == null || projectModel.ProjectStaffs.Count == 0)
            && (projectModel.ProjectClients == null || projectModel.ProjectClients.Count == 0))
            ProjectUsers = new List<ProjectUser>();
    }
}