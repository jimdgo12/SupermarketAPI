namespace SupermarketAPI.Dtos
{
    public class DetailSalesResponseDto
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public string Message { get; set; } = "";
    }
}