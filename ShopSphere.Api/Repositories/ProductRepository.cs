using Microsoft.EntityFrameworkCore;
using ShopSphere.API.Data;
using ShopSphere.API.Models;
using ShopSphere.API.Repositories.Interfaces;
using System.Threading.Tasks;

namespace ShopSphere.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _Context;

        public ProductRepository(AppDbContext context)
        {
            _Context = context;
        }

        public  IQueryable<Product> GetAll() 
        {
        return  _Context.Products.AsQueryable();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _Context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            await _Context.Products.AddAsync(product);
        }

        public void Update(Product product)
        {
            _Context.Products.Update(product);
        }

        public void Delete(Product product)
        {
            _Context.Products.Remove(product);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _Context.Products.AnyAsync(p => p.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _Context.SaveChangesAsync();
        }
    }
}
