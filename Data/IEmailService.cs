// Interfaces/IEmailService.cs
namespace QuizManager.Data
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email);
    }
}
