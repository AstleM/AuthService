namespace AuthService.Enums
{
    public enum TokenType: byte
    {
        AuthenticationToken  = 1,
        EmailConfirmationToken = 2,
        PasswordResetToken = 3
    }
}
