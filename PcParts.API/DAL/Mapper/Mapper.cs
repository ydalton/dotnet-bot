using PcParts.API.DAL.Dto;
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
            Email = order.Email,
            Phone = order.Phone,
            Street = order.Street,
            City = order.City,
            ZipCode = order.ZipCode,
            IsDelivery = order.IsDelivery,
            Products = order.Products.Select(product => ProductToDto(product)).ToList()
        };
    }
}