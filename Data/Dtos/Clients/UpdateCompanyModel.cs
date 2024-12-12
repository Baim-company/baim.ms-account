namespace PersonalAccount.API.Models.Dtos.Clients;
public record UpdateCompanyModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CompanyName { get; set; } = string.Empty;
    public string Voen { get; set; } = string.Empty;
    public string LegalForm { get; set; } = string.Empty;
    public string LegalAddress { get; set; } = string.Empty;
    public string LegalRepresentative { get; set; } = string.Empty;
    public bool IsNational { get; set; }

    public List<Guid> TypeOfActivityIds { get; set; } = new List<Guid>();

    public string Logo { get; set; } = string.Empty;
}