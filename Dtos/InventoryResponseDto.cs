namespace SupermarketAPI.Dtos
{
    public class InventoryResponseDto
    {
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public int StockQuantity { get; set; }
        public string Message { get; set; } = "";
    }
}
