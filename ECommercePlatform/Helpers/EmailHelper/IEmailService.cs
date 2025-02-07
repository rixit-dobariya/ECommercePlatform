namespace ECommercePlatform.Helpers.EmailHelper
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }
}
