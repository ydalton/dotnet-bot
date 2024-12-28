using System;
using PcParts.API.Dto;

namespace CoreBot.Dto;

public class ReturnOrderRequest
{
    public string OrderId { get; set; }
    public bool IsCash { get; set; }
    public string ReasonId { get; set; }
}