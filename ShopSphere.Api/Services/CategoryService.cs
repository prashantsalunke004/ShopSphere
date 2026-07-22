using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ShopSphere.API.Data;
using ShopSphere.API.DTOs;
using ShopSphere.API.Interfaces;
using ShopSphere.API.Models;
using System.ComponentModel;

namespace ShopSphere.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private const string CategoryCacheKey = "Category";
        public CategoryService(AppDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task CreateCategoryAsync(CreateCategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            _memoryCache.Remove(CategoryCacheKey);
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            if (_memoryCache.TryGetValue(CategoryCacheKey,out List<CategoryDto> ? cachedCategory))
            {
                return cachedCategory;
            }

            var categories = await _context.Categories.AsNoTracking().Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,

            }).ToListAsync();

            _memoryCache.Set(CategoryCacheKey, categories,TimeSpan.FromMinutes(10));

            return categories;

        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.AsNoTracking().Where(c => c.Id == id).Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,

            }).FirstOrDefaultAsync();
            return category;
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            var category =await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }
            category.Name = categoryDto.Name;

            await _context.SaveChangesAsync();
            _memoryCache.Remove(CategoryCacheKey);
            return true;

        }


        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            _memoryCache.Remove(CategoryCacheKey);
            return true;

        }
    }
}
