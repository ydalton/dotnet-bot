using System;
using System.Collections.Generic;

namespace PcParts.API.Dto;

public class ReasonResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public ICollection<ReturnOrderResponse> ReturnOrders { get; set; }
}