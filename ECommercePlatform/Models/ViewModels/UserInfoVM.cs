namespace ECommercePlatform.Models.ViewModels
{
    //to display some user info in layout file
    public class UserInfoVM
    {
        public int WishlistCount { get; set; }
        public string ProfilePictureUrl { get;set; }
        public IEnumerable<CartItem> CartItems { get; set; }
        public decimal ShippingCharge { get; set; } = 0;
        public decimal Total { get; set; }
    }
}
