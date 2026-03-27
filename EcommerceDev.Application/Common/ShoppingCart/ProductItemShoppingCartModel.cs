namespace EcommerceDev.Application.Common.ShoppingCart
{
    public class ProductItemShoppingCartModel
    {
        public ProductItemShoppingCartModel(Guid idProduct, int quantity)
        {
            IdProduct = idProduct;
            Quantity = quantity;
        }

        public Guid IdProduct { get; set; }
        public int Quantity { get; set; }
    }
}
