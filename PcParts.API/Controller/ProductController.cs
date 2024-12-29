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
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProducts(string? q)
    {
        IEnumerable<Product> products;

        if (q == null)
        {
            products = await _productRepository.GetAllAsync();
        }
        else
        {
            products = await _productRepository.GetAsync(filter: p => p.Name.Contains(q));
        }
        List<ProductResponse> productResponses = products.Select(product => Mapper.ProductToDto(product)).ToList();
        return Ok(productResponses);
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