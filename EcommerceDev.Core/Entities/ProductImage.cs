using EcommerceDev.Core.Entities;

public class ProductImage : BaseEntity
{
    protected ProductImage() { }
    public ProductImage(string identifier, string path, bool isVisible, Guid idProduct)
    {
        Identifier = identifier;
        Path = path;
        IsVisible = isVisible;
        IdProduct = idProduct;
    }

    public string Identifier { get; set; }
    public string Path { get; set; }
    public bool IsVisible { get; set; }
    public Guid IdProduct { get; set; }
}
