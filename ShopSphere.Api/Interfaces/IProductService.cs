using ShopSphere.API.DTOs;
using ShopSphere.API.Helpers;

namespace ShopSphere.API.Interfaces
{
    public interface IProductService
    {
        Task<bool> CreateProductAsync(CreateProductDto productDto);
        Task<PagedResult<ProductDto>> GetAllProductAsync(ProductQueryDto query);

        Task<ProductDto> GetProductByIdAsync(int Id);

        Task<bool> UpdateProductAsync(int id, UpdateProductDto productDto);

        Task<bool> DeleteProductAsync(int id);
    }
}
