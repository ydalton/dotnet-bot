using System;

namespace CoreBot;

public class ReturnOrderDetails
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public string ReasonId { get; set; }
    public bool IsCash { get; set; }
}