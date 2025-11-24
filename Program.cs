using Microsoft.Data.SqlClient;
using System.Data;
using SupermarketAPI.Repositories;
using SupermarketAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔍 Prueba de conexión a la base de datos
using (var testConnection = new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")))
{
    testConnection.Open();
    Console.WriteLine("✅ Conexión abierta correctamente desde Program.cs");
}

// Registro de dependencias (Inyección de dependencias)
builder.Services.AddControllers();

// 🔑 Registro de IDbConnection para inyección en repositorios
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios y servicios de Categorías
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Repositorios y servicios de Productos
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Repositorios y servicios de Sucursales (Branches)
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IBranchService, BranchService>();

// Repositorios y servicios de Empleados (Employees)
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Repositorios y servicios de Clientes (Customers)
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
