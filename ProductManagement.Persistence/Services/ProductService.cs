using Microsoft.AspNetCore.Http;
using ProductManagement.Core.DTOs.ApiResponses;
using ProductManagement.Core.DTOs.Product;
using ProductManagement.Core.Interfaces.Repositories;
using ProductManagement.Core.Interfaces.Services;
using ProductManagement.Domain.Entities;
using ProductManagement.Persistence.Services.Common;

namespace ProductManagement.Persistence.Services;

public class ProductService : BaseService, IProductService
{
    private readonly IReadRepository<Product> _readRepository;
    private readonly IWriteRepository<Product> _writeRepository;

    public ProductService(IRepositoryManager repositoryManager)
    {
        _readRepository = repositoryManager.GetReadRepository<Product>();
        _writeRepository = repositoryManager.GetWriteRepository<Product>();
    }

    /// <summary>
    ///     Gets all products asynchronously
    /// </summary>
    /// <returns>List of products</returns>
    public async Task<ApiResponse<IEnumerable<ListProductDto>>> GetProductsAsync()
    {
        var products = await _readRepository.GetAllAsync();
        var mappedProducts = ObjectMapper.Map<IEnumerable<ListProductDto>>(products);
        return ApiResponse<IEnumerable<ListProductDto>>.Success(mappedProducts, StatusCodes.Status200OK);
    }

    /// <summary>
    ///     Gets a product by id asynchronously
    /// </summary>
    /// <param name="id">Product id</param>
    /// <returns></returns>
    public async Task<ApiResponse<ListProductDto>> GetProductByIdAsync(Guid id)
    {
        var product = await _readRepository.GetAsync(b => b.Id.Equals(id));
        var mappedProduct = ObjectMapper.Map<ListProductDto>(product);
        return ApiResponse<ListProductDto>.Success(mappedProduct, StatusCodes.Status200OK);
    }

    /// <summary>
    ///     Adds a product asynchronously
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<ApiResponse<NoContentDto>> AddProductAsync(AddProductDto product)
    {
        var mappedProduct = ObjectMapper.Map<Product>(product);
        await _writeRepository.AddAsync(mappedProduct);
        return ApiResponse<NoContentDto>.Success(StatusCodes.Status201Created);
    }

    /// <summary>
    ///     Updates a product asynchronously
    /// </summary>
    /// <param name="product"></param>
    /// <exception cref="Exception"></exception>
    public async Task<ApiResponse<NoContentDto>> UpdateProductAsync(UpdateProductDto product)
    {
        var existingProduct = await _readRepository.GetAsync(p => p.Id.Equals(product.Id), enableTracking: false);
        if (existingProduct is null)
            throw new Exception($"Product with id {product.Id} not found");

        var mappedProduct = ObjectMapper.Map(product, existingProduct);
        await _writeRepository.UpdateAsync(mappedProduct);
        return ApiResponse<NoContentDto>.Success(StatusCodes.Status204NoContent);
    }

    /// <summary>
    ///     Deletes a product asynchronously
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public async Task<ApiResponse<NoContentDto>> DeleteProductAsync(Guid id)
    {
        var existingProduct = await _readRepository.GetAsync(p => p.Id.Equals(id), enableTracking: false);
        if (existingProduct is null)
            throw new Exception($"Product with id {id} not found");

        await _writeRepository.DeleteAsync(existingProduct);
        return ApiResponse<NoContentDto>.Success(StatusCodes.Status204NoContent);
    }
}