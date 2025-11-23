using System.Data;
using System.Data.SqlClient; // 👈 proveedor clásico, estable
using Dapper;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(IConfiguration configuration, ILogger<CategoryRepository> logger)
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

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = "SELECT Id, Name, Description, IsActive FROM Categories";
                return await connection.QueryAsync<Category>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en GetAllAsync");
                throw;
            }
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = "SELECT Id, Name, Description, IsActive FROM Categories WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en GetByIdAsync con ID {Id}", id);
                throw;
            }
        }

        public async Task<int> CreateAsync(Category category)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                _logger.LogInformation("📦 Datos recibidos en CreateAsync → {@Category}", category);

                var sql = @"INSERT INTO Categories (Name, Description, IsActive) 
                            VALUES (@Name, @Description, @IsActive);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

                var newId = await connection.ExecuteScalarAsync<int>(sql, category);
                _logger.LogInformation("✅ Categoría creada con ID {Id}", newId);
                return newId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en CreateAsync");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                _logger.LogInformation("📦 Datos recibidos en UpdateAsync → {@Category}", category);

                var sql = @"UPDATE Categories 
                            SET Name = @Name, Description = @Description, IsActive = @IsActive 
                            WHERE Id = @Id";

                var rowsAffected = await connection.ExecuteAsync(sql, category);
                _logger.LogInformation("✅ Filas afectadas en UpdateAsync: {Rows}", rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en UpdateAsync para la categoría con ID {Id}", category.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = "DELETE FROM Categories WHERE Id = @Id";
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
