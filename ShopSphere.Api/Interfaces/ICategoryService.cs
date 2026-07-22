using ShopSphere.API.DTOs;
namespace ShopSphere.API.Interfaces
{
    public interface ICategoryService
    {
        Task CreateCategoryAsync(CreateCategoryDto categoryDto);
        Task<List<CategoryDto>> GetAllCategoriesAsync();

        Task<CategoryDto?> GetCategoryByIdAsync(int id);

        Task<bool> UpdateCategoryAsync(int id, CategoryDto categoryDto);

        Task<bool> DeleteCategoryAsync(int id);
    }
}
