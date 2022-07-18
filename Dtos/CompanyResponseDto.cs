namespace AuthService.Dtos
{
    public class CompanyResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UserResponseDto CreatedBy { get; set; }
    }
}
