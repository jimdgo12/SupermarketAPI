using System.Data;
using System.Data.SqlClient; 
using Dapper;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(IConfiguration configuration, ILogger<EmployeeRepository> logger)
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

        // 🔹 Obtener todos los empleados
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = @"SELECT Id, IdentificationNumber, FirstName, MiddleName, LastName1, LastName2, 
                                   RoleId, BranchId
                            FROM Employees";

                return await connection.QueryAsync<Employee>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en GetAllAsync");
                throw;
            }
        }

        // 🔹 Obtener empleado por Id
        public async Task<Employee?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = @"SELECT Id, IdentificationNumber, FirstName, MiddleName, LastName1, LastName2, 
                                   RoleId, BranchId
                            FROM Employees 
                            WHERE Id = @Id";

                return await connection.QueryFirstOrDefaultAsync<Employee>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en GetByIdAsync con ID {Id}", id);
                throw;
            }
        }

        // 🔹 Crear empleado
        public async Task<int> CreateAsync(Employee employee)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                _logger.LogInformation("📦 Datos recibidos en CreateAsync → {@Employee}", employee);

                var sql = @"INSERT INTO Employees (IdentificationNumber, FirstName, MiddleName, LastName1, LastName2, RoleId, BranchId)
                            VALUES (@IdentificationNumber, @FirstName, @MiddleName, @LastName1, @LastName2, @RoleId, @BranchId);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

                var newId = await connection.ExecuteScalarAsync<int>(sql, employee);
                _logger.LogInformation("✅ Empleado creado con ID {Id}", newId);
                return newId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en CreateAsync");
                throw;
            }
        }

        // 🔹 Actualizar empleado
        public async Task<bool> UpdateAsync(Employee employee)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                _logger.LogInformation("📦 Datos recibidos en UpdateAsync → {@Employee}", employee);

                var sql = @"UPDATE Employees 
                            SET IdentificationNumber = @IdentificationNumber,
                                FirstName = @FirstName,
                                MiddleName = @MiddleName,
                                LastName1 = @LastName1,
                                LastName2 = @LastName2,
                                RoleId = @RoleId,
                                BranchId = @BranchId
                            WHERE Id = @Id";

                var rowsAffected = await connection.ExecuteAsync(sql, employee);
                _logger.LogInformation("✅ Filas afectadas en UpdateAsync: {Rows}", rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en UpdateAsync para el empleado con ID {Id}", employee.Id);
                throw;
            }
        }

        // 🔹 Eliminar empleado
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = "DELETE FROM Employees WHERE Id = @Id";
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
