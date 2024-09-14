using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Core.Attributes;
using ProductManagement.Core.Consts;
using ProductManagement.Core.DTOs.ApiResponses;
using ProductManagement.Core.DTOs.Product;
using ProductManagement.Core.Enums;
using ProductManagement.Core.Interfaces.Services;

namespace ProductManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<ListProductDto>>> Get()
    {
        var products = await _productService.GetProductsAsync();
        return products;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ListProductDto>> GetById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return product;
    }

    [HttpPost]
    public async Task<ApiResponse<NoContentDto>> Create(AddProductDto product)
    {
        var response = await _productService.AddProductAsync(product);
        return response;
    }

    [HttpPut]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConsts.Products, ActionType = ActionType.Updating, Definition = "Update Product")]
    public async Task<ApiResponse<NoContentDto>> Update(UpdateProductDto product)
    {
        var response = await _productService.UpdateProductAsync(product);
        return response;
    }

    [HttpDelete("{id}")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConsts.Products, ActionType = ActionType.Deleting, Definition = "Delete Product")]
    public async Task<ApiResponse<NoContentDto>> Delete(Guid id)
    {
        var response = await _productService.DeleteProductAsync(id);
        return response;
    }
}