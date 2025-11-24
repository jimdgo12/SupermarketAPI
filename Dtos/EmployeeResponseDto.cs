namespace SupermarketAPI.Dtos
{
    public class EmployeeResponseDto
    {
        public int Id { get; set; }
        public string IdentificationNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName1 { get; set; } = string.Empty;
        public string? LastName2 { get; set; }
        public int RoleId { get; set; }
        public int BranchId { get; set; }        
        public string Message { get; set; } = string.Empty;
    }
}
