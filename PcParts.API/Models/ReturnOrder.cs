namespace PcParts.API.Models;

public class ReturnOrder
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string RefundOption { get; set; }
    public Reason? Reason { get; set; }
}