using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Entities.Agiles.Projects;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Clients;
public class Company
{
    public Guid Id { get; set; }

    public string? CompanyName { get; set; }
    public string? Voen { get; set; }
    public string? LegalForm { get; set; }
    public string? LegalAddress { get; set; }
    public string? LegalRepresentative { get; set; }
    public bool IsNational { get; set; }
    public bool IsPublic { get; set; }
     


    public ICollection<Project>? Projects { get; set; } 
    public ICollection<CompanyProduct>? CompanyProducts { get; set; }

     

    [JsonIgnore]
    public ICollection<Client>? Clients { get; set; } 
    public ICollection<CompanyTypeOfActivity>? CompanyTypeOfActivities { get; set; }



    public string LogoImagePath { get; set; } = "data:image/png;base64,";


    public Company()
    {
        Id = Guid.NewGuid();
    }
    public Company(CompanyModel companyModel,VoenModel voenModel)
    {
        Id = Guid.NewGuid();
        IsNational = companyModel.IsNational;

        Voen = voenModel.Voen;
        CompanyName = voenModel.CompanyName;
        LegalForm = voenModel.LegalForm;
        LegalAddress = voenModel.LegalAddress;
        LegalRepresentative = voenModel.LegalRepresentative;

        LogoImagePath = companyModel.LogoImagePath;
    }
}