using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(IConfiguration configuration, ILogger<CustomerRepository> logger)
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

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = @"SELECT Id, IdentificationNumber, FirstName, MiddleName, LastName1, LastName2, Email, Phone, Address 
                            FROM Customers";

                return await connection.QueryAsync<Customer>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en GetAllAsync");
                throw;
            }
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = @"SELECT Id, IdentificationNumber, FirstName, MiddleName, LastName1, LastName2, Email, Phone, Address 
                            FROM Customers WHERE Id = @Id";

                return await connection.QueryFirstOrDefaultAsync<Customer>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en GetByIdAsync con ID {Id}", id);
                throw;
            }
        }

        public async Task<int> CreateAsync(Customer customer)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                _logger.LogInformation("📦 Datos recibidos en CreateAsync → {@Customer}", customer);

                var sql = @"INSERT INTO Customers 
                            (IdentificationNumber, FirstName, MiddleName, LastName1, LastName2, Email, Phone, Address)
                            VALUES 
                            (@IdentificationNumber, @FirstName, @MiddleName, @LastName1, @LastName2, @Email, @Phone, @Address);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

                var newId = await connection.ExecuteScalarAsync<int>(sql, customer);
                _logger.LogInformation("✅ Cliente creado con ID {Id}", newId);
                return newId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en CreateAsync");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                _logger.LogInformation("📦 Datos recibidos en UpdateAsync → {@Customer}", customer);

                var sql = @"UPDATE Customers SET 
                            IdentificationNumber = @IdentificationNumber,
                            FirstName = @FirstName,
                            MiddleName = @MiddleName,
                            LastName1 = @LastName1,
                            LastName2 = @LastName2,
                            Email = @Email,
                            Phone = @Phone,
                            Address = @Address
                            WHERE Id = @Id";

                var rowsAffected = await connection.ExecuteAsync(sql, customer);
                _logger.LogInformation("✅ Filas afectadas en UpdateAsync: {Rows}", rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en UpdateAsync para el cliente con ID {Id}", customer.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = "DELETE FROM Customers WHERE Id = @Id";
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
