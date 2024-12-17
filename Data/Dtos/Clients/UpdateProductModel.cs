namespace PersonalAccount.API.Models.Dtos.Clients;
public record UpdateProductModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = false;
    public string ImagePath { get; set; } = string.Empty;
}