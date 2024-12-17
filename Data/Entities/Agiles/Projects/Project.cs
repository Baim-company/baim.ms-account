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


    
    public string DesignThemeImagePath { get; set; }
    public string ProjectAvatarImagePath { get; set; } 




    public Guid? CompanyId { get; set; }
    [JsonIgnore]
    public Company? Company { get; set; }

    [JsonIgnore]

    public List<ProjectTiket>? ProjectTikets { get; set; }
    [JsonIgnore]

    public ICollection<ProjectTask>? ProjectTasks { get; set; }


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

        Name = projectModel.Name;
        Description = projectModel.Description;

        ProductId = projectModel.ProductId;
        ProjectManagerId = projectModel.ManagerId;

        CreatedDate = DateTime.UtcNow.AddHours(4);
        FinishDate = projectModel.FinishDate;
        ProjectType = projectModel.ProjectType;

        ProjectAvatarImagePath = projectModel.ProjectAvatarImagePath;
        DesignThemeImagePath = projectModel.DesignThemeImagePath;


        if (projectModel.ProjectType == ProjectType.External) CompanyId = projectModel.CompanyId;
        else projectModel.ProjectClients = null;

        if ((projectModel.ProjectStaffs == null || projectModel.ProjectStaffs.Count == 0)
            && (projectModel.ProjectClients == null || projectModel.ProjectClients.Count == 0))
            ProjectUsers = new List<ProjectUser>();
    }
}