namespace SupermarketAPI.Models
{
    public class Inventory
    {
        // Claves compuestas: BranchId + ProductId
        public int BranchId { get; set; }
        public int ProductId { get; set; }

        // Cantidad en stock
        public int StockQuantity { get; set; }

        // Relaciones de navegación (opcional, si usas EF Core)
        public Branch? Branch { get; set; }
        public Product? Product { get; set; }
    }
}
