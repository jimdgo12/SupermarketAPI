namespace SupermarketAPI.Dtos
{
    public class BranchResponseDto
    {
        public int Id { get; set; }
        public string Nit { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string City { get; set; } = "";

        // Mensaje adicional para respuestas de la API
        public string Message { get; set; } = "";
    }
}
