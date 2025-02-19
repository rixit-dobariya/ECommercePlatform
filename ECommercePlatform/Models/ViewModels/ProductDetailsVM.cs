namespace ECommercePlatform.Models.ViewModels
{
    public class ProductDetailsVM
    {
        public Product Product { get; set; }
        List<ProductDisplay> RelatedProducts { get; set; }
        public int CartQuantity { get; set; }
        public bool IsInWishlist { get; set; }
        public bool HasOrdered { get; set; }
        public double AverageRating { get; set; }
        public List<Review> Reviews { get; set; }
        public Review Review { get; set; }
       
    }
}
