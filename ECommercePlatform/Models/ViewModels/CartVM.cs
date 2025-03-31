namespace ECommercePlatform.Models.ViewModels
{
    public class CartVM
    {
        public IEnumerable<CartItem> CartItems { get; set; }    
        public decimal Total { get; set; }
        public decimal ShippingCharge { get; set; }
        public bool IsCouponApplied { get; set; }
        public string CouponCode { get; set; }

    }
}
