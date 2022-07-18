using AuthService.Models;

namespace AuthService.Services
{
    public interface IPasswordService
    {
        public byte[] GenerateSalt(int length);
        public byte[] HashPassword(string password, byte[] salt, int iterations, int length);
        public bool CheckPasswordValid(string password, byte[] passwordHash, byte[] salt, int iterations, int length);
        public bool ValidatePassword(string password, PasswordRequirements passwordRequirements);
    }
}
