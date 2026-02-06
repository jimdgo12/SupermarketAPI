namespace SupermarketAPI.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public int BranchId { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }

        // Relación para recibir el detalle en el mismo objeto
        public List<SaleDetail> Details { get; set; } = new List<SaleDetail>();
    }

    public class SaleDetail
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}