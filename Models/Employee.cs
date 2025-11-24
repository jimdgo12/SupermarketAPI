public class Employee
{
    public int Id { get; set; }
    public string IdentificationNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName1 { get; set; } = string.Empty;
    public string? LastName2 { get; set; }
    public int RoleId { get; set; }
    public int BranchId { get; set; }
}
