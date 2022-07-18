using AuthService.Models;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AuthService.Services
{
    public class PasswordService : IPasswordService
    {
        public bool CheckPasswordValid(string password, byte[] passwordHash, byte[] salt, int iterations, int length)
        {
            byte[] testPasswordHash = HashPassword(password, salt, iterations, length);

            return ByteArraysEqual(passwordHash, testPasswordHash);
        }

        public byte[] GenerateSalt(int length)
        {
            var salt = new byte[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        public byte[] HashPassword(string password, byte[] salt, int iterations, int length)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return deriveBytes.GetBytes(length);
            }
        }

        public bool ValidatePassword(string password, PasswordRequirements passwordRequirements)
        {
            string errorMessage = string.Empty;

            if (password == null || password.Length == 0)
                throw new ArgumentException("Password is empty");

            if (passwordRequirements == null)
                return true;

            if(passwordRequirements.RequiresUpperCase)
            {
                string regexPattern = @".*[A-Z].*";
                Match match = Regex.Match(password, regexPattern);

                if (!match.Success)
                    errorMessage += "Password must include an upper case character" + Environment.NewLine;
            }

            if (passwordRequirements.RequiresLowerCase)
            {
                string regexPattern = @".*[a-z].*";
                Match match = Regex.Match(password, regexPattern);

                if (!match.Success)
                    errorMessage += "Password must include a lower case character" + Environment.NewLine;
            }

            if (passwordRequirements.RequiresNumericValue)
            {
                string regexPattern = @".*[0-9].*";
                Match match = Regex.Match(password, regexPattern);

                if (!match.Success)
                    errorMessage += "Password must include a numeric character" + Environment.NewLine;
            }

            if (passwordRequirements.RequiresSpecialCharacter)
            {
                string regexPattern = @".*[-+_!@#$%^&*.,?].*";
                Match match = Regex.Match(password, regexPattern);

                if (!match.Success)
                    errorMessage += "Password must include a special character" + Environment.NewLine;
            }

            if (passwordRequirements.MinimumLength > password.Length)
                errorMessage += "Password length must be greater than or equal to " + passwordRequirements.MinimumLength.ToString() + Environment.NewLine;

            if (passwordRequirements.CannotContainStrings.Length > 0)
            {
                foreach(string invalidString in passwordRequirements.CannotContainStrings)
                {
                    string regexPattern = @".*" + invalidString + @".*";
                    Match match = Regex.Match(password, regexPattern);

                    if (match.Success)
                        errorMessage += "Password cannot include string " + invalidString + Environment.NewLine;
                }
            }

            if (errorMessage.Length > 0)
                throw new ArgumentException(errorMessage);

            return true;
        }

        private bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }
    }
}
