namespace SupermarketAPI.Dtos
{
    public class SaleResponseDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime SaleDate { get; set; }
        public string Message { get; set; } = "";

        
    }
}