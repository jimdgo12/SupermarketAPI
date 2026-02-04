using Dapper;
using Microsoft.Data.SqlClient;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly string _connectionString;

        // ✅ Constructor recibe IConfiguration para obtener la cadena de conexión
        public InventoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("⚠️ La cadena de conexión 'DefaultConnection' no está configurada en appsettings.json.");
            }
            Console.WriteLine($"🔑 Cadena de conexión usada en InventoryRepository: {_connectionString}");
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // ✅ Crear inventario
        public async Task<bool> CreateAsync(Inventory inventory)
        {
            using var connection = CreateConnection();
            var sql = @"INSERT INTO Inventory (BranchId, ProductId, StockQuantity)
                        VALUES (@BranchId, @ProductId, @StockQuantity)";
            var rows = await connection.ExecuteAsync(sql, inventory);
            return rows > 0;
        }

        // ✅ Obtener todo el inventario
        public async Task<IEnumerable<Inventory>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var sql = @"SELECT BranchId, ProductId, StockQuantity FROM Inventory";
            return await connection.QueryAsync<Inventory>(sql);
        }

        // ✅ Obtener inventario por BranchId y ProductId
        public async Task<Inventory?> GetByIdAsync(int branchId, int productId)
        {
            using var connection = CreateConnection();
            var sql = @"SELECT BranchId, ProductId, StockQuantity 
                        FROM Inventory 
                        WHERE BranchId = @BranchId AND ProductId = @ProductId";
            return await connection.QueryFirstOrDefaultAsync<Inventory>(sql, new { BranchId = branchId, ProductId = productId });
        }

        // ✅ Obtener inventario por ProductId (todas las sucursales)
        public async Task<IEnumerable<Inventory>> GetByProductAsync(int productId)
        {
            using var connection = CreateConnection();
            var sql = @"SELECT BranchId, ProductId, StockQuantity 
                        FROM Inventory 
                        WHERE ProductId = @ProductId";
            return await connection.QueryAsync<Inventory>(sql, new { ProductId = productId });
        }

        // ✅ Actualizar inventario completo
        public async Task<bool> UpdateAsync(Inventory inventory)
        {
            using var connection = CreateConnection();
            var sql = @"UPDATE Inventory 
                        SET StockQuantity = @StockQuantity 
                        WHERE BranchId = @BranchId AND ProductId = @ProductId";
            var rows = await connection.ExecuteAsync(sql, inventory);
            return rows > 0;
        }

        // ✅ Eliminar inventario
        public async Task<bool> DeleteAsync(int branchId, int productId)
        {
            using var connection = CreateConnection();
            var sql = @"DELETE FROM Inventory 
                        WHERE BranchId = @BranchId AND ProductId = @ProductId";
            var rows = await connection.ExecuteAsync(sql, new { BranchId = branchId, ProductId = productId });
            return rows > 0;
        }

        // ✅ Sumar stock existente
        public async Task<bool> AddStockAsync(int branchId, int productId, int quantityToAdd)
        {
            using var connection = CreateConnection();
            var sql = @"UPDATE Inventory 
                        SET StockQuantity = StockQuantity + @QuantityToAdd 
                        WHERE BranchId = @BranchId AND ProductId = @ProductId";
            var rows = await connection.ExecuteAsync(sql, new { BranchId = branchId, ProductId = productId, QuantityToAdd = quantityToAdd });
            return rows > 0;
        }
    }
}
