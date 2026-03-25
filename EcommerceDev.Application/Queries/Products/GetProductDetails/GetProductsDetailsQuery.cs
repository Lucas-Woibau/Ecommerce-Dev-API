namespace EcommerceDev.Application.Queries.Products.GetProductDetails
{
    public class GetProductsDetailsQuery
    {
        public GetProductsDetailsQuery(Guid idProduct)
        {
            IdProduct = idProduct;
        }

        public Guid IdProduct { get; set; }
    }
}
