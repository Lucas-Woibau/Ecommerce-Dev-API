using EcommerceDev.Core.Entities;

public class ProductImage : BaseEntity
{
    protected ProductImage() { }
    public ProductImage(bool isVisible, Guid idProduct)
    {
        IsVisible = isVisible;
        IdProduct = idProduct;
    }

    public void ConfigureIdentifier(string extension)
    {
        Identifier = Id.ToString();
        Path = $"{IdProduct}/{Id}.{extension}";
    }

    public string Identifier { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public bool IsVisible { get; set; }
    public Guid IdProduct { get; set; }
}
