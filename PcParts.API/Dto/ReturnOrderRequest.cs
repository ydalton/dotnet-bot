namespace PcParts.API.Dto;

public class ReturnOrderRequest
{
    public string? OrderId { get; set; }
    public bool IsCash { get; set; }
    public string? ReasonId { get; set; }
}