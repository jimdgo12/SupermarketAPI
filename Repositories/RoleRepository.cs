using System.Data;
using System.Data.SqlClient; // 👈 proveedor clásico, estable
using Dapper;
using Microsoft.Extensions.Logging;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<RoleRepository> _logger;

        public RoleRepository(IConfiguration configuration, ILogger<RoleRepository> logger)
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

        // 🔹 Obtener todos los roles
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = "SELECT Id, Name, Description FROM Roles";
                return await connection.QueryAsync<Role>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en GetAllAsync");
                throw;
            }
        }

        // 🔹 Obtener rol por ID
        public async Task<Role?> GetByIdAsync(int id)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = "SELECT Id, Name, Description FROM Roles WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Role>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en GetByIdAsync con ID {Id}", id);
                throw;
            }
        }

        // 🔹 Crear rol
        public async Task<int> CreateAsync(Role role)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                _logger.LogInformation("📦 Datos recibidos en CreateAsync → {@Role}", role);

                var sql = @"INSERT INTO Roles (Name, Description) 
                            VALUES (@Name, @Description);
                            SELECT CAST(SCOPE_IDENTITY() as int);";

                var newId = await connection.ExecuteScalarAsync<int>(sql, role);
                _logger.LogInformation("✅ Rol creado con ID {Id}", newId);
                return newId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en CreateAsync");
                throw;
            }
        }

        // 🔹 Actualizar rol
        public async Task<bool> UpdateAsync(Role role)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                _logger.LogInformation("📦 Datos recibidos en UpdateAsync → {@Role}", role);

                var sql = @"UPDATE Roles 
                            SET Name = @Name, Description = @Description 
                            WHERE Id = @Id";

                var rowsAffected = await connection.ExecuteAsync(sql, role);
                _logger.LogInformation("✅ Filas afectadas en UpdateAsync: {Rows}", rowsAffected);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Error en UpdateAsync para el rol con ID {Id}", role.Id);
                throw;
            }
        }

        // 🔹 Eliminar rol
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using var connection = CreateSqlConnection();
                await connection.OpenAsync();

                var sql = "DELETE FROM Roles WHERE Id = @Id";
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
