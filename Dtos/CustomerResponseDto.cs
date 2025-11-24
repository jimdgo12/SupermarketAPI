namespace SupermarketAPI.Dtos
{
    public class CustomerResponseDto
    {
        public int Id { get; set; }
        public string IdentificationNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName1 { get; set; } = string.Empty;
        public string? LastName2 { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }   
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Message { get; set; }
    }
}
