using System;
using System.Collections.Generic;

namespace CoreBot.Dto;

public class OrderResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public bool IsDelivery { get; set; }
    public IEnumerable<ProductResponse> Products { get; set; }
}