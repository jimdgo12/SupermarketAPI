using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<BranchRepository> _logger;

        public BranchRepository(IConfiguration configuration, ILogger<BranchRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("❌ No se encontró la cadena de conexión 'DefaultConnection'.");
            _logger = logger;
        }

        private SqlConnection CreateSqlConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            using var connection = CreateSqlConnection();
            var sql = "SELECT Id, Nit, Name, Address, Phone, Email, City FROM Branches";
            return await connection.QueryAsync<Branch>(sql);
        }

        public async Task<Branch?> GetByIdAsync(int id)
        {
            using var connection = CreateSqlConnection();
            var sql = "SELECT Id, Nit, Name, Address, Phone, Email, City FROM Branches WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Branch>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(Branch branch)
        {
            using var connection = CreateSqlConnection();
            var sql = @"INSERT INTO Branches (Nit, Name, Address, Phone, Email, City)
                        VALUES (@Nit, @Name, @Address, @Phone, @Email, @City);
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            return await connection.ExecuteScalarAsync<int>(sql, branch);
        }

        public async Task<bool> UpdateAsync(Branch branch)
        {
            using var connection = CreateSqlConnection();
            var sql = @"UPDATE Branches 
                        SET Nit = @Nit, Name = @Name, Address = @Address, Phone = @Phone, Email = @Email, City = @City
                        WHERE Id = @Id";
            var rows = await connection.ExecuteAsync(sql, branch);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = CreateSqlConnection();
            var sql = "DELETE FROM Branches WHERE Id = @Id";
            var rows = await connection.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }
    }
}
