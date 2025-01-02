using Microsoft.AspNetCore.Mvc;
using PcParts.API.DAL;
using PcParts.API.DAL.Mapper;
using PcParts.API.Dto;
using PcParts.API.Models;

namespace PcParts.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class ReturnOrdersController : ControllerBase
{
    private IRepository<ReturnOrder> _returnOrderRepository;
    private IRepository<Order> _orderRepository;
    private IRepository<Reason> _reasonRepository;

    public ReturnOrdersController(IRepository<ReturnOrder> returnOrderRepository, IRepository<Order> orderRepository,
        IRepository<Reason> reasonRepository)
    {
        _returnOrderRepository = returnOrderRepository;
        _orderRepository = orderRepository;
        _reasonRepository = reasonRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReturnOrderResponse>>> GetAllReturnOrders()
    {
        IEnumerable<ReturnOrder> returnOrders = await _returnOrderRepository.GetAsync(includes: r => r.Reason);
        List<ReturnOrderResponse> returnOrderResponses =
            returnOrders.Select(returnOrder => Mapper.ReturnOrderToDto(returnOrder)).ToList();
        return Ok(returnOrderResponses);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReturnOrderResponse>> GetReturnOrderById(Guid id)
    {
        var returnOrder = await _returnOrderRepository.GetAsync(includes: ro => ro.Reason, filter: ro => ro.Id == id);

        if (returnOrder.First() == null)
        {
            return NotFound();
        }

        ReturnOrderResponse returnOrderResponse = Mapper.ReturnOrderToDto(returnOrder.First());
        return Ok(returnOrderResponse);
    }
    
    [HttpGet("orderNumber/{id:guid}")]
    public async Task<ActionResult<bool>> CheckOrderNumberExists(Guid id)
    {
        var returnOrder = await _returnOrderRepository.GetAsync(filter: ro => ro.OrderId == id);
        Order? order = await _orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            return Ok(true);
        }
        
        if (!returnOrder.Any())
        {
            return Ok(false);
        }
        
        return Ok(true);
    }

    [HttpPost]
    public async Task<ActionResult<ReturnOrderResponse>> CreateReturnOrder(ReturnOrderRequest returnOrderRequest)
    {
        var orderId = Guid.Empty;
        Reason reason = null;
        
        if (returnOrderRequest.OrderId != null)
        {
            orderId = Guid.Parse(returnOrderRequest.OrderId);
        }
        
        if (returnOrderRequest.Reason != null)
        {
            var reasons = (await _reasonRepository.GetAsync(filter: r => r.Code == returnOrderRequest.Reason)).ToList();

            if (reasons.Count == 0)
            {
                return BadRequest("The requested reason does not exist.");
            }

            reason = reasons.First();
        }

        var returnOrder = new ReturnOrder()
        {
            OrderId = orderId,
            Reason = reason,
            RefundOption = returnOrderRequest.RefundOption
        };

        _returnOrderRepository.Insert(returnOrder);
        await _orderRepository.SaveAsync();

        return CreatedAtAction(nameof(GetReturnOrderById), new { id = returnOrder.Id }, Mapper.ReturnOrderToDto(returnOrder));
    }
}