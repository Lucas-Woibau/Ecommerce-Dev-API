namespace EcommerceDev.Application.Queries.Products.GetAllProducts
{
    public class GetAllProductsItemViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
