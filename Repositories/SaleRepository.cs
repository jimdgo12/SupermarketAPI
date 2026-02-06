using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<SaleRepository> _logger;

        public SaleRepository(IConfiguration configuration, ILogger<SaleRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("❌ No se encontró la cadena de conexión 'DefaultConnection'.");
            _logger = logger;
        }

        private SqlConnection CreateSqlConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();
                var sql = @"SELECT Id, SaleDate, TotalAmount, BranchId, CustomerId, EmployeeId FROM Sales ORDER BY SaleDate DESC";
                return await connection.QueryAsync<Sale>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en GetAllAsync de Ventas");
                throw;
            }
        }

        public async Task<Sale?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();
                var sql = @"SELECT Id, SaleDate, TotalAmount, BranchId, CustomerId, EmployeeId FROM Sales WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Sale>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en GetByIdAsync con ID {Id}", id);
                throw;
            }
        }

        public async Task<int> CreateAsync(Sale sale)
        {
            using var connection = CreateSqlConnection();
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                _logger.LogInformation("📦 Procesando venta...");

                // 1. Insertar Cabecera
                var sqlSale = @"INSERT INTO Sales (TotalAmount, BranchId, CustomerId, EmployeeId)
                                VALUES (@TotalAmount, @BranchId, @CustomerId, @EmployeeId);
                                SELECT CAST(SCOPE_IDENTITY() as int);";

                var saleId = await connection.ExecuteScalarAsync<int>(sqlSale, sale, transaction);

                // 2. Insertar Detalles y Actualizar Stock
                foreach (var detail in sale.Details)
                {
                    detail.SaleId = saleId;

                    await connection.ExecuteAsync(@"INSERT INTO SaleDetails (SaleId, ProductId, Quantity, UnitPrice, Subtotal)
                                                    VALUES (@SaleId, @ProductId, @Quantity, @UnitPrice, @Subtotal)",
                                                    detail, transaction);

                    // 🔍 RASTREO DE STOCK: Verificamos si la actualización afectó alguna fila
                    var rowsAffected = await connection.ExecuteAsync(@"UPDATE Inventory SET StockQuantity = StockQuantity - @Quantity 
                                                                     WHERE BranchId = @BranchId AND ProductId = @ProductId",
                                                                     new { detail.Quantity, sale.BranchId, detail.ProductId }, transaction);

                    if (rowsAffected == 0)
                    {
                        // Si no afectó nada, el error es la combinación Branch/Product
                        throw new Exception($"EL ERROR ES: No existe el Producto {detail.ProductId} en la Sucursal {sale.BranchId} dentro de la tabla Inventory.");
                    }
                }

                transaction.Commit();
                return saleId;
            }
            catch (SqlException ex)
            {
                transaction.Rollback();

                // 🎯 EL MENSAJE ACERTADO: Traduciendo errores de SQL para el desarrollador
                if (ex.Number == 547) // Error de Llave Foránea
                {
                    if (ex.Message.Contains("FK_Sales_Branches"))
                        throw new Exception($"EL ERROR ES: El BranchId {sale.BranchId} no existe en la base de datos.");

                    if (ex.Message.Contains("FK_Sales_Customers"))
                        throw new Exception($"EL ERROR ES: El CustomerId {sale.CustomerId} no existe.");

                    if (ex.Message.Contains("FK_Sales_Employees"))
                        throw new Exception($"EL ERROR ES: El EmployeeId {sale.EmployeeId} no existe.");
                }

                throw new Exception($"ERROR_SQL_{ex.Number}: {ex.Message}");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "💥 Error en CreateAsync");
                throw new Exception(ex.Message); // Mandamos el mensaje exacto al desarrollador
            }
        }
    }
}