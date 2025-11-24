namespace SupermarketAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string IdentificationNumber { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string? MiddleName { get; set; }
        public string LastName1 { get; set; } = default!;
        public string? LastName2 { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }  
    }
}
