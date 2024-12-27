using System.Threading.Tasks;
using CoreBot.DialogDetails;
using CoreBot.Dto;

namespace CoreBot.Services;

public class OrderService
{
    public static async Task<OrderResponse> PostOrder(OrderDetails orderDetails)
    {
        return await ApiService<OrderDetails>.PostAsync<OrderResponse>("Order", orderDetails);
    }
}