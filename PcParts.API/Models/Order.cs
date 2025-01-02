namespace PcParts.API.Models;

public class Order
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string DeliveryOption { get; set; }
    public IEnumerable<Product>? Products { get; set; }
}