using EcommerceDev.Core.Entities;

namespace EcommerceDev.Application.Queries.Products.GetProductDetails
{
    public class ProductDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } 
        public string Brand { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int ReviewsCount { get; set; }

        public static ProductDetailsViewModel FromEntity(Product product)
        {
            return new ProductDetailsViewModel()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Brand = product.Brand,
                Quantity = product.Quantity,
                CategoryName = product.Category.Title,
                ReviewsCount = product.Reviews.Count
            };
        }
    }
}
