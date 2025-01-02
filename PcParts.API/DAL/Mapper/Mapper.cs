using PcParts.API.DAL.Dto;
using PcParts.API.Dto;
using PcParts.API.Models;

namespace PcParts.API.DAL.Mapper;

public static class Mapper
{
    public static ProductResponse ProductToDto(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity,
            Description = product.Description,
            Category = product.Category
        };
    }
    
    public static OrderResponse OrderToDto(Order order)
    {
        return new OrderResponse()
        {
            Id = order.Id,
            Name = order.Name,
            Email = order.Email,
            Phone = order.Phone,
            Street = order.Street,
            City = order.City,
            ZipCode = order.ZipCode,
            DeliveryOption = order.DeliveryOption,
            Products = order.Products.Select(product => ProductToDto(product)).ToList()
        };
    }
    
    public static ReasonResponse ReasonToDto(Reason reason)
    {
        return new ReasonResponse()
        {
            Id = reason.Id,
            Name = reason.Name,
            Code = reason.Code,
        };
    }
    
    public static ReturnOrderResponse ReturnOrderToDto(ReturnOrder returnOrder)
    {
        return new ReturnOrderResponse()
        {
            Id = returnOrder.Id,
            OrderId = returnOrder.OrderId,
            Reason = ReasonToDto(returnOrder.Reason),
            RefundOption = returnOrder.RefundOption,
        };
    }
}