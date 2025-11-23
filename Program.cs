using Microsoft.Data.SqlClient;
using SupermarketAPI.Repositories;
using SupermarketAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔍 Prueba de conexión a la base de datos
using (var testConnection = new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")))
{
    testConnection.Open();
    Console.WriteLine("✅ Conexión abierta correctamente desde Program.cs");
    // El using se encarga de cerrar la conexión automáticamente
}

// Registro de dependencias (Inyección de dependencias)
builder.Services.AddControllers();

// Repositorios y servicios de Categorías
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Repositorios y servicios de Productos
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

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
