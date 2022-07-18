namespace AuthService.Dtos
{
    public class CompanyCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public UserCreateDto CreatedBy { get; set; }
    }
}
