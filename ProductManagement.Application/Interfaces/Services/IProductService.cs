using ProductManagement.Core.DTOs.ApiResponses;
using ProductManagement.Core.DTOs.Product;

namespace ProductManagement.Core.Interfaces.Services;

public interface IProductService : IBaseService
{
    public Task<ApiResponse<IEnumerable<ListProductDto>>> GetProductsAsync();
    public Task<ApiResponse<ListProductDto>> GetProductByIdAsync(Guid id);
    public Task<ApiResponse<NoContentDto>> AddProductAsync(AddProductDto product);
    public Task<ApiResponse<NoContentDto>> UpdateProductAsync(UpdateProductDto product);
    public Task<ApiResponse<NoContentDto>> DeleteProductAsync(Guid id);
}