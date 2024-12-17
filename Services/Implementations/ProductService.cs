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
    private readonly string _baseImageUrl;
    private readonly IFileService _fileService;
    private readonly AgileDbContext _baimDbContext; 


    public ProductService(AgileDbContext baimDbContext,
        IConfiguration configuration,
        IFileService fileService)
    {
        _baimDbContext = baimDbContext;
        _fileService = fileService;
        _baseImageUrl = configuration["BaseImageUrl"]
            ?? throw new Exception("BaseImageUrl is not configured");
    }



    public async Task<Response<Product>> GetProductByIdAsync(Guid id)
    {
        var product = await _baimDbContext.Products
            .FirstOrDefaultAsync(p => p.IsPublic == true && p.Id == id);

        if (product == null)
            throw new PersonalAccountException(PersonalAccountErrorType.ProductNotFound,
                $"Error!Failed to find product with id: {id}");


        product.ImagePath = $"{_baseImageUrl}/{product.ImagePath}".Replace("\\", "/");

        return new Response<Product>("Success!", product);
    }





    public async Task<List<Product>> GetFilteredProductsAsync(string? onSearch, string? onFilter)
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

        foreach (var product in products)
        {
            product.ImagePath = $"{_baseImageUrl}/{product.ImagePath}".Replace("\\", "/");
        }

        return products;
    }



    public async Task<Response<Product>> AddProductAsync(ProductModel model)
    {
        var productExist = await _baimDbContext.Products
            .FirstOrDefaultAsync(n => n.Name == model.Name && n.ProductType == model.ProductType);

        if (productExist != null)
            throw new PersonalAccountException(PersonalAccountErrorType.ProductNotFound,
                "Product with this name and product type already exist!");

        var newProduct = new Product(model);

        await _baimDbContext.Products.AddAsync(newProduct);
        await _baimDbContext.SaveChangesAsync();

        return new Response<Product>("Successfylly created!", newProduct);
    }



    public async Task<Response<Product>> UpdateProductAsync(UpdateProductModel model)
    {
        var productExist = await _baimDbContext.Products.FindAsync(model.Id);
        if (productExist == null)
            throw new PersonalAccountException(PersonalAccountErrorType.ProductNotFound, $"Error!Failed to find product with id: {model.Id}");


        await _baimDbContext.Products
            .Where(u => u.Id == model.Id)
            .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.Name, model.Name)
                .SetProperty(u => u.Description, model.Description)
                .SetProperty(u => u.ProductType, model.ProductType)
                .SetProperty(u => u.IsPublic, model.IsPublic)
                .SetProperty(u => u.ImagePath, model.ImagePath));

        await _baimDbContext.SaveChangesAsync();

        return new Response<Product>("Success!", new Product());
    }




    public async Task<Response<Product>> DeleteProductAsync(Guid id)
    {
        var productExist = await _baimDbContext.Products.FindAsync(id);
        if (productExist == null)
            throw new PersonalAccountException(PersonalAccountErrorType.ProductNotFound, 
                $"Error!Failed to find product with id: {id}");


        var result = _fileService.DeleteFile(productExist.ImagePath);
        if (!result.Data) return new Response<Product>(result.Message);


        await _baimDbContext.Products
            .Where(v => v.Id == id)
            .ExecuteDeleteAsync();

        await _baimDbContext.SaveChangesAsync();

        return new Response<Product>("Success!", new Product());

    }
}