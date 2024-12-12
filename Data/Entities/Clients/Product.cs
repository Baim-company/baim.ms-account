using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Entities.Agiles.Projects;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Clients;

public class Product
{
    public Guid Id { get; set; }
    public string Id1C { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public string ProductType { get; set; }

    public string CombinedImage => $"{ImageType}{Convert.ToBase64String(Image)}";
    [JsonIgnore]
    public byte[] Image { get; set; }
    [JsonIgnore]
    public string ImageType { get; set; } = "data:image/png;base64,";
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
        Id1C = productModel.Id1C;
        Name = productModel.Name;
        Description = productModel.Description;
        IsPublic = productModel.IsPublic;
        ProductType = productModel.ProductType;


        if (!string.IsNullOrEmpty(productModel.Image))
        {
            Image = Convert.FromBase64String(productModel.Image!.Split(',')[1]);
            ImageType = productModel.Image.Split(',')[0] + ",";
        }
    }
}