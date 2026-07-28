using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ShopSphere.API.Data;
using ShopSphere.API.DTOs;
using ShopSphere.API.Interfaces;
using ShopSphere.API.Models;
using System.ComponentModel;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ShopSphere.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;
        private const string CategoryCacheKey = "Category";
        private readonly ILogger _logger;
        public CategoryService(AppDbContext context, IMemoryCache memoryCache, IMapper mapper,ILogger logger)
        {
            _context = context;
            _memoryCache = memoryCache;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateCategoryAsync(CreateCategoryDto categoryDto)
        {
            //var category = new Category
            //{
            //    Name = categoryDto.Name
            //};
            var category = _mapper.Map<Category>(categoryDto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Category Created Successfully");
            _memoryCache.Remove(CategoryCacheKey);
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            if (_memoryCache.TryGetValue(CategoryCacheKey,out List<CategoryDto> ? cachedCategory))
            {
                return cachedCategory;
            }

            //var categories = await _context.Categories.AsNoTracking().Select(c => new CategoryDto
            //{
            //    Id = c.Id,
            //    Name = c.Name,

            //}).ToListAsync();

            var categories = await _context.Categories.AsNoTracking().ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).ToListAsync();

            _memoryCache.Set(CategoryCacheKey, categories,TimeSpan.FromMinutes(10));
            _logger.LogInformation("Fetched All Categories and Saved In Cache also");
            return categories;

        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            //var category = await _context.Categories.AsNoTracking().Where(c => c.Id == id).Select(c => new CategoryDto
            //{
            //    Id = c.Id,
            //    Name = c.Name,

            //}).FirstOrDefaultAsync();
            var category = await _context.Categories.AsNoTracking().Where(c => c.Id == id).ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            return category;
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryDto categoryDto)
        {
            var category =await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }
            //category.Name = categoryDto.Name;
            _mapper.Map(categoryDto,category);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Category Updated of Id:{categoryId}", id);
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
            _logger.LogInformation("Category Deleted of Id:{categoryId}" , id);
            _memoryCache.Remove(CategoryCacheKey);
            return true;

        }
    }
}
