namespace ECommercePlatform.Models.ViewModels
{
    public class ContactVM
    {
        public Response Response { get; set; }
        public string[] PhoneNumbers { get; set; }
        public string[] Emails { get; set; }
        public string Address { get; set; }
    }
}
