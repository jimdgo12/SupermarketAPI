using Microsoft.Data.SqlClient;

namespace SupermarketAPI.Seeders
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            // 🔹 Categorías
            var checkCategories = connection.CreateCommand();
            checkCategories.CommandText = "SELECT COUNT(*) FROM Categories";
            var categoryCount = Convert.ToInt32(await checkCategories.ExecuteScalarAsync());

            if (categoryCount == 0)
            {
                var insertCategories = connection.CreateCommand();
                insertCategories.CommandText = @"
                    INSERT INTO Categories (Name) VALUES
                    ('Bebidas'), ('Lácteos'), ('Panadería'), ('Carnes'), ('Verduras'), ('Aseo')";
                await insertCategories.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Categorías insertadas.");
            }

            // 🔹 Roles
            var checkRoles = connection.CreateCommand();
            checkRoles.CommandText = "SELECT COUNT(*) FROM Roles";
            var roleCount = Convert.ToInt32(await checkRoles.ExecuteScalarAsync());

            if (roleCount == 0)
            {
                var insertRoles = connection.CreateCommand();
                insertRoles.CommandText = @"
                    INSERT INTO Roles (Name) VALUES
                    ('Administrador'), ('Cajero'), ('Supervisor'), ('Repartidor')";
                await insertRoles.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Roles insertados.");
            }

            // 🔹 Sucursales
            var checkBranches = connection.CreateCommand();
            checkBranches.CommandText = "SELECT COUNT(*) FROM Branches";
            var branchCount = Convert.ToInt32(await checkBranches.ExecuteScalarAsync());

            if (branchCount == 0)
            {
                var insertBranches = connection.CreateCommand();
                insertBranches.CommandText = @"
                    INSERT INTO Branches (Name, Address) VALUES
                    ('Sucursal Norte', 'Calle 10 #5-20'),
                    ('Sucursal Sur', 'Carrera 15 #8-30')";
                await insertBranches.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Sucursales insertadas.");
            }

            // 🔹 Clientes
            var checkCustomers = connection.CreateCommand();
            checkCustomers.CommandText = "SELECT COUNT(*) FROM Customers";
            var customerCount = Convert.ToInt32(await checkCustomers.ExecuteScalarAsync());

            if (customerCount == 0)
            {
                var insertCustomers = connection.CreateCommand();
                insertCustomers.CommandText = @"
                    INSERT INTO Customers (Name, Email) VALUES
                    ('Juan Pérez', 'juan@example.com'),
                    ('Ana Gómez', 'ana@example.com')";
                await insertCustomers.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Clientes insertados.");
            }

            // 🔹 Empleados
            var checkEmployees = connection.CreateCommand();
            checkEmployees.CommandText = "SELECT COUNT(*) FROM Employees";
            var employeeCount = Convert.ToInt32(await checkEmployees.ExecuteScalarAsync());

            if (employeeCount == 0)
            {
                var insertEmployees = connection.CreateCommand();
                insertEmployees.CommandText = @"
                    INSERT INTO Employees (Name, RoleId, BranchId) VALUES
                    ('Carlos Ruiz', 1, 1),
                    ('Laura Torres', 2, 2)";
                await insertEmployees.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Empleados insertados.");
            }

            // 🔹 Productos
            var checkProducts = connection.CreateCommand();
            checkProducts.CommandText = "SELECT COUNT(*) FROM Products";
            var productCount = Convert.ToInt32(await checkProducts.ExecuteScalarAsync());

            if (productCount == 0)
            {
                var insertProducts = connection.CreateCommand();
                insertProducts.CommandText = @"
                    INSERT INTO Products (Name, Price, CategoryId) VALUES
                    ('Leche Entera', 3500, 2),
                    ('Pan Integral', 2800, 3),
                    ('Jugo de Naranja', 4200, 1)";
                await insertProducts.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Productos insertados.");
            }

            Console.WriteLine("🌱 Seeding completo.");
        }
    }
}
