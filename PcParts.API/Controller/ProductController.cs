using Microsoft.AspNetCore.Mvc;
using PcParts.API.DAL;
using PcParts.API.DAL.Dto;
using PcParts.API.DAL.Mapper;
using PcParts.API.Models;

namespace PcParts.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private IRepository<Product> _productRepository;
    
    public ProductController(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> Get()
    {
        IEnumerable<Product> products = await _productRepository.GetAllAsync();
        List<ProductResponse> productResponses = products.Select(product => Mapper.ProductToDto(product)).ToList();
        return Ok(productResponses);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> Get(Guid id)
    {
        Product? product = await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }
        
        ProductResponse productResponse = Mapper.ProductToDto(product);
        return Ok(productResponse);
    }
}