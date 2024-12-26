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
    private IRepository<Product> _productRepository;

    public OrderController(IRepository<Order> orderRepository, IRepository<Product> productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
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

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder(OrderRequest orderRequest)
    {
        IEnumerable<Product> products = (await _productRepository.GetAsync(filter: p => p.Name.Contains(orderRequest.ProductName))).ToList();

        if (!products.Any())
        {
            return BadRequest("The requested product does not exist.");
        } 
        else if (products.Count() > 1)
        {
            return BadRequest("Too many matches.");
        }
        Product product = products.First();
        
        Order order = new Order
        {
            Email = orderRequest.EmailAddress,
            Street = orderRequest.StreetAddress,
            City = orderRequest.City,
            ZipCode = orderRequest.PostCode,
            IsDelivery = orderRequest.IsDelivery,
            Products = new List<Product> { product },
        };

        _orderRepository.Insert(order);
        await _orderRepository.SaveAsync();

        return Ok(Mapper.OrderToDto(order));
    }
}