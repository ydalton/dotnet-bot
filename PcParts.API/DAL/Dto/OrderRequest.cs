namespace PcParts.API.Controller;

public class OrderRequest
{
    public string ProductName { get; set; }
    public string Name { get; set; }
    public bool IsDelivery { get; set; }
    public string EmailAddress { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
}