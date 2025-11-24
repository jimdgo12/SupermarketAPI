namespace SupermarketAPI.Dtos
{
    public class RoleResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public string Message { get; set; } = "";
    }
}
