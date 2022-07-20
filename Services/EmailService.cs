namespace AuthService.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> logger;

        public EmailService(ILogger<EmailService> logger)
        {
            this.logger = logger;
        }
        public void SendConfirmEmailEmail(string email, string emailToken)
        {
            logger.LogWarning("Email service not set up yet");
        }
    }
}
