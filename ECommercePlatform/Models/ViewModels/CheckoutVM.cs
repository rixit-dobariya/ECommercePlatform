namespace ECommercePlatform.Models.ViewModels
{
    public class CheckoutVM
    {
        public IEnumerable<Address> Addresses { get; set; }

        public IEnumerable<CartItem> CartItems { get; set; }
        public decimal Total { get; set; }
        public decimal ShippingCharge { get; set; }
        public Address Address{ get; set; }
        public int ShippingAddressId { get; set; }
    }
}
