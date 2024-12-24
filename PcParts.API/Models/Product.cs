namespace PcParts.API.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
    public int Quantity { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
}