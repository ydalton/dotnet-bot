using Microsoft.AspNetCore.Mvc;
using PcParts.API.DAL;
using PcParts.API.DAL.Dto;
using PcParts.API.DAL.Mapper;
using PcParts.API.Models;

namespace PcParts.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private IRepository<Order> _orderRepository;

    public OrderController(IRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders()
    {
        IEnumerable<Order> orders = await _orderRepository.GetAsync(includes: o => o.Products);
        List<OrderResponse> orderResponses = orders.Select(order => Mapper.OrderToDto(order)).ToList();

        return Ok(orderResponses);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResponse>> GetOrderById(Guid id)
    {
        var order = await _orderRepository.GetAsync(includes: o => o.Products, filter: o => o.Id == id);

        if (order.First() == null)
        {
            return NotFound();
        }

        OrderResponse orderResponse = Mapper.OrderToDto(order.First());
        return Ok(orderResponse);
    }
}