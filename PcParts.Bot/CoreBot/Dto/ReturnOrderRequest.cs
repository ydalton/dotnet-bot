using System;
using PcParts.API.Dto;

namespace CoreBot.Dto;

public class ReturnOrderRequest
{
    public string OrderId { get; set; }
    public string RefundOption { get; set; }
    public string Reason { get; set; }
}