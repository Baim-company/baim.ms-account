namespace PersonalAccount.API.Models.Dtos.Clients;
public record ProductModel
{
    public string Id1C { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public string Image { get; set; } = string.Empty;
}