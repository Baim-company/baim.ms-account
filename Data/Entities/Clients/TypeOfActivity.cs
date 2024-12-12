using PersonalAccount.API.Models.Dtos.Clients;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Clients;
public class TypeOfActivity
{
    public Guid Id { get; set; }
    public string? Id1C { get; set; } 
    public string Title { get; set; }


    [JsonIgnore]
    public List<CompanyTypeOfActivity>? CompanyTypeOfActivities { get; set; }


    public TypeOfActivity()
    {
        Id = Guid.NewGuid();
    }
    public TypeOfActivity(TypeOfActivityModel typeOfActivityModel)
    {
        Id = Guid.NewGuid();

        Id1C = typeOfActivityModel.Id1C;
        Title = typeOfActivityModel.Title;
    }
}