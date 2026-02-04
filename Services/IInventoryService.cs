using SupermarketAPI.Models;

namespace SupermarketAPI.Services
{
    public interface IInventoryService
    {
        Task<bool> CreateInventoryAsync(Inventory inventory);
        Task<IEnumerable<Inventory>> GetAllInventoryAsync();
        Task<Inventory?> GetInventoryByIdAsync(int branchId, int productId);
        Task<IEnumerable<Inventory>> GetInventoryByProductAsync(int productId); 
        Task<bool> UpdateInventoryAsync(Inventory inventory);
        Task<bool> DeleteInventoryAsync(int branchId, int productId);
        Task<bool> AddStockAsync(int branchId, int productId, int quantityToAdd); 
    }
}
