using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommercePlatform.Models.ViewModels
{
    public class HomeVM
    {
        public List<ProductDisplay> NewArrivals { get; set; }
        public List<ProductDisplay> BestSellers { get; set; }
        public List<ProductDisplay> SaleItems { get; set; }
    }
}
