using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService newsService)
    {
        _productService = newsService;
    }


    [HttpGet("Product/Id/{id}")]
    public async Task<ActionResult<Product>> Product(Guid id)
    {
        var result = await _productService.GetProductByIdAsync(id);

        if (result.Data == null) return NotFound(result.Message);

        return Ok(result.Data);
    }




    [HttpGet("Products")]
    public async Task<ActionResult<List<Product>>> Products(
        [FromQuery] string? onSearch = null,
        [FromQuery] string? onFilter = null)
    {
        var products = await _productService.GetFilteredProductsAsync(onSearch, onFilter);

        if (products == null) return NotFound($"There are no products");

        return Ok(products);
    }




    [Authorize(Policy = "AdminOnly")]
    [HttpPost("Create")]
    public async Task<ActionResult<string>> Create([FromBody] ProductModel model)
    {
        var result = await _productService.AddProductAsync(model);

        if (result.Data == null) return BadRequest($"{result.Message}");

        return Ok(result.Message);
    }




    [Authorize(Policy = "AdminOnly")]
    [HttpPut("Update")]
    public async Task<ActionResult<string>> Update([FromBody] UpdateProductModel model)
    {

        var result = await _productService.UpdateProductAsync(model);

        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }




    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("Delete")]
    public async Task<ActionResult<string>> Delete([FromHeader]Guid id)
    {
        var result = await _productService.DeleteProductAsync(id);

        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}