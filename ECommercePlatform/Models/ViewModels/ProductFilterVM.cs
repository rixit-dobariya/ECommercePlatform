namespace ECommercePlatform.Models.ViewModels
{
    public class ProductFilterVM
    {
        public string? Query { get; set; } // For search
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int Rating { get; set; }
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<ProductDisplay> ProductDisplays { get; set; }

        // Pagination properties
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
    }


}
