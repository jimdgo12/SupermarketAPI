using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IInventoryRepository
    {
        Task<bool> CreateAsync(Inventory inventory);
        Task<IEnumerable<Inventory>> GetAllAsync();
        Task<Inventory?> GetByIdAsync(int branchId, int productId);
        Task<IEnumerable<Inventory>> GetByProductAsync(int productId); 
        Task<bool> UpdateAsync(Inventory inventory);
        Task<bool> DeleteAsync(int branchId, int productId);
        Task<bool> AddStockAsync(int branchId, int productId, int quantityToAdd); 
    }
}
