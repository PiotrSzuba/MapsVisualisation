using Database.Models;
using System;
namespace SharedResourcesClientFront.Data;

public class RegionService
{
    private readonly HttpClient client = new HttpClient();
    private readonly string? ApiUrl = Environment.GetEnvironmentVariable("Api");
    public async Task<string> GetRegionsAsync()
    {
        return await client.GetStringAsync(ApiUrl + "/test");
    }
}
