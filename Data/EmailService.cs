
namespace QuizManager.Data
{
    public class EmailService : IEmailService
    {
        public Task SendVerificationEmailAsync(string email)
        {
            // Implement your email sending logic here
            // For example, using an SMTP client or a third-party email service API
            return Task.CompletedTask;
        }
    }
}
