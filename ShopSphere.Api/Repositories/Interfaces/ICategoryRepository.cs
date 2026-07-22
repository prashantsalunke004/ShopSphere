namespace ShopSphere.API.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<bool> ExistsAsync(int id);
    }
}
