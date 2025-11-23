namespace SupermarketAPI.Dtos
{
    public class ProductResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public bool IsActive { get; set; }

        // Mensaje adicional para respuestas de la API
        public string Message { get; set; } = "";
    }
}
