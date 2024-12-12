namespace PersonalAccount.API.Models.Dtos.Clients;
public record ExternalUserCompanyDto
{
    public ExternalUserDto User { get; set; } = new ExternalUserDto();
    public Guid CompanyId { get; set; } 
}