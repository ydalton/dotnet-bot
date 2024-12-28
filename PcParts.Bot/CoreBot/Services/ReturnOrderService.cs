using System;
using System.Threading.Tasks;
using CoreBot.Dto;
using PcParts.API.Dto;

namespace CoreBot.Services;

public class ReturnOrderService
{
    public static async Task<bool> CheckOrderNumberExistsInReturnOrder(Guid id)
    {
        return await ApiService<bool>.GetAsync($"returnOrders/orderNumber/{id}");
    }
    
    public static async Task InsertReturnOrderAsync(ReturnOrderRequest returnOrderRequest)
    {
        await ApiService<ReturnOrderRequest>.PostAsync<ReturnOrderResponse>("returnOrders", returnOrderRequest);
    }
}