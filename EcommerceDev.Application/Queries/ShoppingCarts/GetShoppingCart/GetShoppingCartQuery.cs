namespace EcommerceDev.Application.Queries.ShoppingCarts.GetShoppingCart
{
    public class GetShoppingCartQuery
    {
        public GetShoppingCartQuery(Guid idCustomer)
        {
            IdCustomer = idCustomer;
        }

        public Guid IdCustomer { get; set; }
    }
}
