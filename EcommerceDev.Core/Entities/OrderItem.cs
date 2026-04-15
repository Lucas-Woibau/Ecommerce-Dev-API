namespace EcommerceDev.Core.Entities;

public class OrderItem : BaseEntity
{
    protected OrderItem() { }
    public OrderItem(Guid idProduct, int quantity)
    {
        IdProduct = idProduct;
        Quantity = quantity;
    }

    public Guid IdProduct { get; set; }
    public Product Product { get; set; }
    public Guid IdOrder { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public OrderItemReview Review { get; set; }

    public void SetItemPrice(decimal price)
    {
        Price = price;
    }
}