using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Clients;

public class TypeOfActivity
{
    public Guid Id { get; set; }
    public string Title { get; set; }


    [JsonIgnore]
    public List<CompanyTypeOfActivity>? CompanyTypeOfActivities { get; set; }


    public TypeOfActivity()
    {
        Id = Guid.NewGuid();
    }
    public TypeOfActivity(string title)
    {
        Id = Guid.NewGuid();
        Title = title;
    }
}