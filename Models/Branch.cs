namespace SupermarketAPI.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public string Nit { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string City { get; set; } = string.Empty;
    }
}
