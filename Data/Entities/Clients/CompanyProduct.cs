using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Clients;
public class CompanyProduct
{
    [JsonIgnore]
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    [JsonIgnore]
    public Guid CompanyId { get; set; }
    [JsonIgnore]
    public Company Company { get; set; }

    public CompanyProduct() { }
    public CompanyProduct(Company company,Product product)
    {
        Company = company;
        CompanyId = company.Id;
        Product = product;
        ProductId = product.Id;
    }
}