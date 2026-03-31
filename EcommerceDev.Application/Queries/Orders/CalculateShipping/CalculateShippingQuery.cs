namespace EcommerceDev.Application.Queries.Orders.CalculateShipping
{
    public class CalculateShippingQuery
    {
        public string ZipCode { get; set; }
        public List<CalculateShippingQueryItem> Items { get; set; }
    }
}
