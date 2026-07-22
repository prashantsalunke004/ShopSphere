using Microsoft.EntityFrameworkCore;
using ShopSphere.API.Data;
using ShopSphere.API.Interfaces;
using ShopSphere.API.Repositories.Interfaces;

namespace ShopSphere.API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;

        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(p => p.Id == id);
        }

    }
}
