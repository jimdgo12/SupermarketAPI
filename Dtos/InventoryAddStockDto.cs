namespace SupermarketAPI.Dtos
{
    public class InventoryAddStockDto
    {
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public int QuantityToAdd { get; set; }
    }
}
