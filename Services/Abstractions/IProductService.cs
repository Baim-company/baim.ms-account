using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Clients;

namespace PersonalAccount.API.Services.Abstractions;
public interface IProductService
{
    public Task<List<Product>> GetFilteredProductsAsync(string? onSearch, string? onFilter);
    public Task<Response<Product>> GetProductByIdAsync(Guid id);
    public Task<Response<Product>> AddProductAsync(ProductModel model);
    public Task<Response<Product>> DeleteProductAsync(Guid id);
    public Task<Response<Product>> UpdateProductAsync(UpdateProductModel model);
}