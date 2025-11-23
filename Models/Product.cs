namespace SupermarketAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public bool IsActive { get; set; }

        // Propiedad de navegación (opcional pero recomendable si usas EF Core)
        public Category? Category { get; set; }
    }
}
