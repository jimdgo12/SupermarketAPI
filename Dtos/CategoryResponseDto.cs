namespace SupermarketAPI.Dtos
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string Message { get; set; } = "";
    }
}
