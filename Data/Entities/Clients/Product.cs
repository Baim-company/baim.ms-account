using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Entities.Agiles.Projects;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Clients;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public string ProductType { get; set; }

    public string ImagePath { get; set; }

    [JsonIgnore]
    public IEnumerable<CompanyProduct>? CompanyProducts { get; set; }
    [JsonIgnore]
    public IEnumerable<Project>? Projects { get; set; }


    public Product()
    {
        Id = Guid.NewGuid();
    }
    public Product(ProductModel productModel)
    {
        Id = Guid.NewGuid();
        Name = productModel.Name;
        Description = productModel.Description;
        IsPublic = productModel.IsPublic;
        ProductType = productModel.ProductType;

        ImagePath = productModel.ImagePath;
    }
}