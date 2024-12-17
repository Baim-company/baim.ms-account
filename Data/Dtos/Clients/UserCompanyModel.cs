namespace PersonalAccount.API.Models.Dtos.Clients;
public record UserCompanyModel
{
    public Guid Id { get; set; }


    public string CompanyName { get; set; } = string.Empty;
    public string Voen { get; set; } = string.Empty;
    public string LegalForm { get; set; } = string.Empty;
    public string LegalAddress { get; set; } = string.Empty;
    public string LegalRepresentative { get; set; } = string.Empty;
    public bool IsNational { get; set; }

     
    public string Image { get; set; } = string.Empty;
}