namespace AuthService.Services
{
    public interface IEmailService
    {
        public void SendConfirmEmailEmail(string email, string emailToken);
    }
}
