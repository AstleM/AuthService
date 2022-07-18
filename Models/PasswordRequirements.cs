namespace AuthService.Models
{
    public class PasswordRequirements
    {
        public bool RequiresLowerCase { get; set; } = true;
        public bool RequiresUpperCase { get; set; } = true;
        public bool RequiresNumericValue { get; set; } = true;
        public bool RequiresSpecialCharacter { get; set; } = true;
        public int MinimumLength { get; set; } = 10;
        public string[] CannotContainStrings { get; set; } = new string[0];
    }
}
