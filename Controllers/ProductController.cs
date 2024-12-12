using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.API.Models.Dtos.Clients;
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


    [HttpGet("{id}")]
    public async Task<IActionResult> Product(Guid id)
    {
        var result = await _productService.GetProductByIdAsync(id);

        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Data);
    }




    [HttpGet("Products")]
    public async Task<IActionResult> Products(
        [FromQuery] string? onSearch = null,
        [FromQuery] string? onFilter = null)
    {
        var products = await _productService.GetFilteredProductsAsync(onSearch, onFilter);

        if (products == null) return BadRequest($"There are no products");

        return Ok(products);
    }




    [Authorize(Policy = "AdminOnly")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ProductModel model)
    {
        var result = await _productService.AddProductAsync(model);

        if (result.Data == null) return BadRequest($"{result.Message}");

        return Ok(result.Data);
    }




    [Authorize(Policy = "AdminOnly")]
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdateProductModel model)
    {

        var result = await _productService.UpdateProductAsync(model);

        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }




    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromHeader]Guid id)
    {
        var result = await _productService.DeleteProductAsync(id);

        if (result.Data == null) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}