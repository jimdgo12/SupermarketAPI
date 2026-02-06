using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public class DetailSalesRepository : IDetailSalesRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<DetailSalesRepository> _logger;

        public DetailSalesRepository(IConfiguration configuration, ILogger<DetailSalesRepository> logger)
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

        public async Task<IEnumerable<DetailSales>> GetAllAsync()
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = "SELECT SaleId, ProductId, Quantity, UnitPrice, Subtotal FROM SaleDetails";
                return await connection.QueryAsync<DetailSales>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en GetAllAsync");
                throw;
            }
        }

        public async Task<DetailSales?> GetByIdAsync(int saleId)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = "SELECT SaleId, ProductId, Quantity, UnitPrice, Subtotal FROM SaleDetails WHERE SaleId = @SaleId";
                return await connection.QueryFirstOrDefaultAsync<DetailSales>(sql, new { SaleId = saleId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en GetByIdAsync con SaleId {SaleId}", saleId);
                throw;
            }
        }

        public async Task<int> CreateAsync(DetailSales detailSales)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                _logger.LogInformation("📦 Datos recibidos en CreateAsync → {@DetailSales}", detailSales);

                var sql = @"INSERT INTO SaleDetails (ProductId, Quantity, UnitPrice, Subtotal) 
                            VALUES (@ProductId, @Quantity, @UnitPrice, @Subtotal);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

                var newId = await connection.ExecuteScalarAsync<int>(sql, detailSales);
                _logger.LogInformation("✅ DetailSales creado con SaleId {Id}", newId);
                return newId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en CreateAsync");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(DetailSales detailSales)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                _logger.LogInformation("📦 Datos recibidos en UpdateAsync → {@DetailSales}", detailSales);

                var sql = @"UPDATE SaleDetails 
                            SET ProductId = @ProductId, Quantity = @Quantity, UnitPrice = @UnitPrice, Subtotal = @Subtotal
                            WHERE SaleId = @SaleId";

                var rowsAffected = await connection.ExecuteAsync(sql, detailSales);
                _logger.LogInformation("✅ Filas afectadas en UpdateAsync: {Rows}", rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en UpdateAsync para SaleId {SaleId}", detailSales.SaleId);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int saleId)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = "DELETE FROM SaleDetails WHERE SaleId = @SaleId";
                var rowsAffected = await connection.ExecuteAsync(sql, new { SaleId = saleId });
                _logger.LogInformation("✅ Filas eliminadas en DeleteAsync: {Rows}", rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en DeleteAsync con SaleId {SaleId}", saleId);
                throw;
            }
        }
    }
}