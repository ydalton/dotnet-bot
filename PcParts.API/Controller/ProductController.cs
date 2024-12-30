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
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts([FromQuery(Name = "q")] string? name, string? category)
    {
        IEnumerable<Product> products;

        /* broken if both name and category are not null */
        if (name != null)
        {
            products = await _productRepository.GetAsync(filter: p => p.Name.Contains(name));
        }
        else if (category != null)
        {
            products = await _productRepository.GetAsync(filter: p => p.Category == category);
        }
        else
        {
            products = await _productRepository.GetAllAsync();
        }
        List<ProductResponse> productResponses = products.Select(product => Mapper.ProductToDto(product)).ToList();
        return Ok(productResponses);
    }

    [HttpGet("Categories")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        var categories = await _productRepository.GetAllAsync();
        
        return categories.Select(c => c.Category).Distinct().ToList();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetProductById(Guid id)
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