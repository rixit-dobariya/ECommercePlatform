namespace ECommercePlatform.Models.ViewModels
{
    public class ProductDisplay
    {
        public decimal SellPrice { get; set; }
        public int Stock { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Discount { get; set; }
        public string? ImageUrl { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }

        public bool IsInWishlist { get; set; } = false;
        public bool IsInCart { get; set; } = false;
        public double AverageRating { get; set; } = 0;
    }
}
