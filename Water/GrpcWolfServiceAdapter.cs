using System.Net.Http;
using System.Net.Http.Json;
using Services;

public class GrpcWolfServiceAdapter
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://127.0.0.1:5000";

    public GrpcWolfServiceAdapter(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public int EnterWolfArea(RabbitDesc rabbit)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/enterWolfArea") { Content = JsonContent.Create(rabbit) };
        var response = _httpClient.Send(request);
        response.EnsureSuccessStatusCode();
        return response.Content.ReadFromJsonAsync<int>().GetAwaiter().GetResult();
    }

    public int SpawnWaterNearWolf(WaterDesc water)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/spawnWaterNearWolf") { Content = JsonContent.Create(water) };
        var response = _httpClient.Send(request);
        response.EnsureSuccessStatusCode();
        return response.Content.ReadFromJsonAsync<int>().GetAwaiter().GetResult();
    }

    public bool UpdateRabbitDistanceToWolf(RabbitDesc rabbit)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"{_baseUrl}/updateRabbitDistanceToWolf") { Content = JsonContent.Create(rabbit) };
        var response = _httpClient.Send(request);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public bool IsRabbitAlive(RabbitDesc rabbit)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/isRabbitAlive") { Content = JsonContent.Create(rabbit) };
        var response = _httpClient.Send(request);
        response.EnsureSuccessStatusCode();
        return response.Content.ReadFromJsonAsync<bool>().GetAwaiter().GetResult();
    }


    public bool IsWaterAlive(WaterDesc water)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/IsWaterAlive") { Content = JsonContent.Create(water) };
        var response = _httpClient.Send(request);
        response.EnsureSuccessStatusCode();
        return response.Content.ReadFromJsonAsync<bool>().GetAwaiter().GetResult();
    }
}
