﻿namespace PcParts.API.Dto;

public class ReturnOrderResponse
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string RefundOption { get; set; }
    public ReasonResponse? Reason { get; set; }
}