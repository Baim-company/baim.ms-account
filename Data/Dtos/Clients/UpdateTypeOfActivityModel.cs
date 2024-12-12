namespace PersonalAccount.API.Models.Dtos.Clients;

public record UpdateTypeOfActivityModel
{
    public Guid Id { get; set; } 
    public string Id1C { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}