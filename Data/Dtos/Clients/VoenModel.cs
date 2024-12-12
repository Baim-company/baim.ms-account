namespace PersonalAccount.API.Models.Dtos.Clients;
public record VoenModel
{
    public string CompanyName { get; set; } = string.Empty;
    public string Voen { get; set; } = string.Empty;
    public string LegalForm { get; set; } = string.Empty;
    public string LegalAddress { get; set; } = string.Empty;
    public string LegalRepresentative { get; set; } = string.Empty;
}