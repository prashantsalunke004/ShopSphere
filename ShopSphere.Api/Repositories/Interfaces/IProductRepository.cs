using ShopSphere.API.Models;

namespace ShopSphere.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IQueryable<Product> GetAll();

        Task<Product?> GetByIdAsync(int id);

        Task AddAsync(Product product);

        void Update(Product product);

        void Delete(Product product);

        Task<bool> ExistsAsync(int id);

        Task SaveChangesAsync();
    }
}
