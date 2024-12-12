using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Clients;
public class CompanyTypeOfActivity
{
    [JsonIgnore]
    public Guid CompanyId { get; set; }
    [JsonIgnore]
    public Company Company { get; set; }

    [JsonIgnore]
    public Guid TypeOfActivityId { get; set; }
    public TypeOfActivity TypeOfActivity { get; set; }

    public CompanyTypeOfActivity() { }
    public CompanyTypeOfActivity(Company company, TypeOfActivity typeOfActivity)
    {
        CompanyId = company.Id;
        Company = company;
        TypeOfActivityId = typeOfActivity.Id;
        TypeOfActivity = typeOfActivity;
    }
}