using System.Collections.Generic;
using System.Threading.Tasks;
using CoreBot.Dto;

namespace CoreBot.Services;

public class ProductService
{
    public static async Task<List<ProductResponse>> GetProducts()
    {
        return await ApiService<List<ProductResponse>>.GetAsync("Product");
    }
}