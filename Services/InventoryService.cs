using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repository;

        public InventoryService(IInventoryRepository repository)
        {
            _repository = repository;
        }

        // ✅ Crear inventario
        public async Task<bool> CreateInventoryAsync(Inventory inventory)
        {
            return await _repository.CreateAsync(inventory);
        }

        // ✅ Obtener todo el inventario
        public async Task<IEnumerable<Inventory>> GetAllInventoryAsync()
        {
            return await _repository.GetAllAsync();
        }

        // ✅ Obtener inventario por BranchId y ProductId
        public async Task<Inventory?> GetInventoryByIdAsync(int branchId, int productId)
        {
            return await _repository.GetByIdAsync(branchId, productId);
        }

        // ✅ Obtener inventario por ProductId (todas las sucursales)
        public async Task<IEnumerable<Inventory>> GetInventoryByProductAsync(int productId)
        {
            return await _repository.GetByProductAsync(productId);
        }

        // ✅ Actualizar inventario completo
        public async Task<bool> UpdateInventoryAsync(Inventory inventory)
        {
            return await _repository.UpdateAsync(inventory);
        }

        // ✅ Eliminar inventario
        public async Task<bool> DeleteInventoryAsync(int branchId, int productId)
        {
            return await _repository.DeleteAsync(branchId, productId);
        }

        // ✅ Sumar stock existente
        public async Task<bool> AddStockAsync(int branchId, int productId, int quantityToAdd)
        {
            return await _repository.AddStockAsync(branchId, productId, quantityToAdd);
        }
    }
}
