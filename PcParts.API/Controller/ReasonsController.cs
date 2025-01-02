using Microsoft.AspNetCore.Mvc;
using PcParts.API.DAL;
using PcParts.API.DAL.Mapper;
using PcParts.API.Dto;
using PcParts.API.Models;

namespace PcParts.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class ReasonsController : ControllerBase
{
    private IRepository<Reason> _reasonRepository;
    
    public ReasonsController(IRepository<Reason> reasonRepository)
    {
        _reasonRepository = reasonRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReasonResponse>>> GetAllReasons()
    {
        IEnumerable<Reason> reasons = await _reasonRepository.GetAsync(orderBy: r => r.OrderBy(x => x.Name));
        List<ReasonResponse> reasonResponses = reasons.Select(r => Mapper.ReasonToDto(r)).ToList();
        return Ok(reasonResponses);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReasonResponse>> GetReasonById(Guid id)
    {
        var reason = await _reasonRepository.GetByIdAsync(id);

        if (reason == null)
        {
            return NotFound();
        }
        
        ReasonResponse reasonResponse = Mapper.ReasonToDto(reason);
        return Ok(reasonResponse);
    }
    
    [HttpGet("by-code/{code}")]
    public async Task<ActionResult<ReasonResponse>> GetCategoryByCode(string code)
    {
        var reason = await _reasonRepository.GetAsync(filter: r => r.Code == code);

        if (!reason.Any())
        {
            return NotFound();
        }

        ReasonResponse reasonResponse = Mapper.ReasonToDto(reason.First());
        return Ok(reasonResponse);
    }
}