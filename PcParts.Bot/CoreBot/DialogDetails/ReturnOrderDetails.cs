using System;

namespace CoreBot;

public class ReturnOrderDetails
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public string Reason { get; set; }
    public string RefundOption { get; set; }
}