namespace AuthService.Models
{
    public class User
    {
        public User()
        {
            CreatedAt = DateTime.Now;
        }

        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool EmailConfirmed { get; set; } = false;
    }
}
