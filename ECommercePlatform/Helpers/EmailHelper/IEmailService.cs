namespace ECommercePlatform.Helpers.EmailHelper
{
    public interface IEmailService
    {
        Task SendEmail(string to, string subject, string body);
    }
}
