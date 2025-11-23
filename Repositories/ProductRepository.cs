using System.Data;
using System.Data.SqlClient; // proveedor clásico, estable
using Dapper;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(IConfiguration configuration, ILogger<ProductRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("❌ No se encontró la cadena de conexión 'DefaultConnection'.");
            _logger = logger;

            _logger.LogInformation("🔍 Constructor ejecutado. Cadena de conexión: {Connection}", _connectionString);
        }

        private SqlConnection CreateSqlConnection()
        {
            var connection = new SqlConnection(_connectionString);
            _logger.LogInformation("🔎 CreateSqlConnection ejecutado. Cadena actual: {Connection}", connection.ConnectionString);
            return connection;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = @"SELECT Id, Name, Description, Price, CategoryId, IsActive 
                            FROM Products";
                return await connection.QueryAsync<Product>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en GetAllAsync");
                throw;
            }
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = @"SELECT Id, Name, Description, Price, CategoryId, IsActive 
                            FROM Products 
                            WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en GetByIdAsync con ID {Id}", id);
                throw;
            }
        }

        public async Task<int> CreateAsync(Product product)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                _logger.LogInformation("📦 Datos recibidos en CreateAsync → {@Product}", product);

                var sql = @"INSERT INTO Products (Name, Description, Price, CategoryId, IsActive) 
                            VALUES (@Name, @Description, @Price, @CategoryId, @IsActive);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

                var newId = await connection.ExecuteScalarAsync<int>(sql, product);
                _logger.LogInformation("✅ Producto creado con ID {Id}", newId);
                return newId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en CreateAsync");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                _logger.LogInformation("📦 Datos recibidos en UpdateAsync → {@Product}", product);

                var sql = @"UPDATE Products 
                            SET Name = @Name, Description = @Description, Price = @Price, 
                                CategoryId = @CategoryId, IsActive = @IsActive 
                            WHERE Id = @Id";

                var rowsAffected = await connection.ExecuteAsync(sql, product);
                _logger.LogInformation("✅ Filas afectadas en UpdateAsync: {Rows}", rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en UpdateAsync para el producto con ID {Id}", product.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = "DELETE FROM Products WHERE Id = @Id";
                var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
                _logger.LogInformation("✅ Filas eliminadas en DeleteAsync: {Rows}", rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en DeleteAsync con ID {Id}", id);
                throw;
            }
        }
    }
}
