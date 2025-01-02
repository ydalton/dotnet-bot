namespace PcParts.API.Dto;

public class ReturnOrderRequest
{
    public string? OrderId { get; set; }
    public string RefundOption { get; set; }
    public string? Reason { get; set; }
}