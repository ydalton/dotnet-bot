using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PcParts.API.Dto;

namespace CoreBot.Services;

public class ReasonService
{
    public static async Task<ReasonResponse> GetReasonByIdAsync(Guid id)
    {
        return await ApiService<ReasonResponse>.GetAsync($"reasons/{id}");
    }
    
    public static async Task<List<ReasonResponse>> GetReasonsAsync()
    {
        return await ApiService<List<ReasonResponse>>.GetAsync("reasons");
    }
    
    public static async Task<ReasonResponse> GetReasonByCodeAsync(string code)
    {
        return await ApiService<ReasonResponse>.GetAsync($"reasons/by-code/{code}");
    }
}