using Global.Infrastructure.Exceptions.PersonalAccount;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Data.DbContexts; 
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Services.Implementations;

public class ProductService : IProductService
{
    private readonly AgileDbContext _baimDbContext;

    public ProductService(AgileDbContext baimDbContext)
    {
        _baimDbContext = baimDbContext;
    }


    public async Task<Response<Product>> AddProductAsync(ProductModel model)
    {
        try
        { 
            var productExist = await _baimDbContext.Products
                .FirstOrDefaultAsync(n => n.Name == model.Name && n.ProductType == model.ProductType);
             
            if (productExist == null) 
                throw new PersonalAccountException(PersonalAccountErrorType.ProductNotFound, "Product with this name and product type already exist!");

            var newProduct = new Product(model);

            await _baimDbContext.Products.AddAsync(newProduct);
            await _baimDbContext.SaveChangesAsync();

            return new Response<Product>("Successfylly created!", newProduct);
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task<Response<Product>> DeleteProductAsync(Guid id)
    {
        try
        {
            var productExist = await _baimDbContext.Products.FindAsync(id);
            if (productExist == null) 
                throw new PersonalAccountException(PersonalAccountErrorType.ProductNotFound, $"Error!Failed to find product with id: {id}");

            await _baimDbContext.Products
                .Where(v => v.Id == id)
                .ExecuteDeleteAsync();

            await _baimDbContext.SaveChangesAsync();

            return new Response<Product>("Success!", new Product());
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<List<Product>> GetFilteredProductsAsync(string? onSearch, string? onFilter)
    {
        try
        {
            var productsQuery = _baimDbContext.Products
                .Where(c => c.IsPublic == true)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(onSearch))
            {
                productsQuery = productsQuery.Where(p =>
                    p.Name!.ToLower().Contains(onSearch.ToLower())
                );
            }
            if (!string.IsNullOrEmpty(onFilter))
            {
                productsQuery = productsQuery.Where(p =>
                    p.ProductType!.ToLower().Contains(onFilter.ToLower())
                );
            }

            var products = await productsQuery.ToListAsync();

            return products;
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task<Response<Product>> GetProductByIdAsync(Guid id)
    {
        try
        {
            var product = await _baimDbContext.Products.FindAsync(id);

            if (product == null) 
                throw new PersonalAccountException(PersonalAccountErrorType.ProductNotFound, $"Error!Failed to find product with id: {id}");

            return new Response<Product>("Success!", product);
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<Response<Product>> UpdateProductAsync(UpdateProductModel model)
    {
        try
        {
            var productExist = await _baimDbContext.Products.FindAsync(model.Id);
            if (productExist == null) 
                throw new PersonalAccountException(PersonalAccountErrorType.ProductNotFound, $"Error!Failed to find product with id: {model.Id}");

            string imageType = model.Image!.Split(',')[0] + ",";
            byte[] imageBytes = Convert.FromBase64String(model.Image!.Split(',')[1]);

            await _baimDbContext.Products
                .Where(u => u.Id == model.Id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(u => u.Id1C, model.Id1C)
                    .SetProperty(u => u.Name, model.Name)
                    .SetProperty(u => u.Description, model.Description)
                    .SetProperty(u => u.ProductType, model.ProductType)
                    .SetProperty(u => u.IsPublic, model.IsPublic)
                    .SetProperty(u => u.Image, imageBytes)
                    .SetProperty(u => u.ImageType, imageType));

            await _baimDbContext.SaveChangesAsync();

            return new Response<Product>("Success!", new Product());
        }
        catch (Exception)
        {
            throw;
        }
    }
}