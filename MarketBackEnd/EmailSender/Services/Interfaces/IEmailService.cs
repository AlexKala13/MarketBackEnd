namespace MarketBackEnd.EmailSender.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordResetEmail(string to, string token);
    }
}
