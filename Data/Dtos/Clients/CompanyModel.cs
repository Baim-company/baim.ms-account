namespace PersonalAccount.API.Models.Dtos.Clients;
public record CompanyModel
{
    public Guid Id { get; set; } 
    public bool IsNational { get; set; }
    public string LogoImagePath { get; set; } = string.Empty;
    
    public List<Guid> TypesOfActivities { get; set; } = new();
    public VoenModel? voenModel { get; set; }
}