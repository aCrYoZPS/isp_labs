using System.Text.Json;
using Lab1.Entities;

namespace Lab1.Services;

public class RateService : IRateService
{
    private readonly HttpClient httpClient;

    public RateService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public IEnumerable<Rate> GetRates(DateTime date)
    {
        var response = httpClient.GetAsync($"?ondate={date:yyyy-MM-dd}&periodicity=0").Result;
        response.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<IEnumerable<Rate>>(response.Content.ReadAsStringAsync().Result) ?? [];
    }
}