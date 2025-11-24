using Microsoft.Data.SqlClient;
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
        }

        private SqlConnection CreateSqlConnection() => new SqlConnection(_connectionString);

        // 🔹 Obtener todos los empleados
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            using var connection = CreateSqlConnection();
            var sql = @"SELECT Id, IdentificationNumber, FirstName, MiddleName, LastName1, LastName2, 
                               RoleId, BranchId
                        FROM Employees";
            return await connection.QueryAsync<Employee>(sql);
        }

        // 🔹 Obtener empleado por Id
        public async Task<Employee?> GetByIdAsync(int id)
        {
            using var connection = CreateSqlConnection();
            var sql = @"SELECT Id, IdentificationNumber, FirstName, MiddleName, LastName1, LastName2, 
                               RoleId, BranchId
                        FROM Employees 
                        WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Employee>(sql, new { Id = id });
        }

        // 🔹 Crear empleado
        public async Task<int> CreateAsync(Employee employee)
        {
            using var connection = CreateSqlConnection();
            var sql = @"INSERT INTO Employees (IdentificationNumber, FirstName, MiddleName, LastName1, LastName2, RoleId, BranchId)
                        VALUES (@IdentificationNumber, @FirstName, @MiddleName, @LastName1, @LastName2, @RoleId, @BranchId);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            return await connection.ExecuteScalarAsync<int>(sql, employee);
        }

        // 🔹 Actualizar empleado
        public async Task<bool> UpdateAsync(Employee employee)
        {
            using var connection = CreateSqlConnection();
            var sql = @"UPDATE Employees 
                        SET IdentificationNumber = @IdentificationNumber,
                            FirstName = @FirstName,
                            MiddleName = @MiddleName,
                            LastName1 = @LastName1,
                            LastName2 = @LastName2,
                            RoleId = @RoleId,
                            BranchId = @BranchId
                        WHERE Id = @Id";
            var rows = await connection.ExecuteAsync(sql, employee);
            return rows > 0;
        }

        // 🔹 Eliminar empleado
        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = CreateSqlConnection();
            var sql = "DELETE FROM Employees WHERE Id = @Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
