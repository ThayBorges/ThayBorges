using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace WasteManagement.Tests;

public class EndpointStatusTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public EndpointStatusTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CollectionPoints_Get_ReturnsHttpStatusCode200()
    {
        var response = await _client.GetAsync("api/collectionpoints?page=1&pageSize=5");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CollectionRequests_GetForPoint_ReturnsHttpStatusCode200()
    {
        var collectionPointId = await GetFirstCollectionPointIdAsync();
        var response = await _client.GetAsync($"api/collection-requests/collection-point/{collectionPointId}?page=1&pageSize=5");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Alerts_Get_ReturnsHttpStatusCode200()
    {
        var response = await _client.GetAsync("api/alerts?page=1&pageSize=5");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task ImpactReports_GetSnapshots_ReturnsHttpStatusCode200()
    {
        var token = await GetTokenAsync("auditor", "auditor@2024");
        var request = new HttpRequestMessage(HttpMethod.Get, "api/impactreports/snapshots");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Auth_Token_ReturnsHttpStatusCode200()
    {
        var response = await _client.PostAsJsonAsync("api/auth/token", new { Username = "planner", Password = "planner@2024" });
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        payload.TryGetProperty("access_token", out var token).Should().BeTrue();
        token.GetString().Should().NotBeNullOrWhiteSpace();
    }

    private async Task<Guid> GetFirstCollectionPointIdAsync()
    {
        var response = await _client.GetAsync("api/collectionpoints?page=1&pageSize=1");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);
        var items = document.RootElement.GetProperty("Items");

        if (items.GetArrayLength() > 0)
        {
            return items[0].GetProperty("Id").GetGuid();
        }

        return await CreateCollectionPointAsync();
    }

    private async Task<Guid> CreateCollectionPointAsync()
    {
        var token = await GetTokenAsync("planner", "planner@2024");
        var request = new HttpRequestMessage(HttpMethod.Post, "api/collectionpoints")
        {
            Content = JsonContent.Create(new
            {
                Code = $"AUTO-{Guid.NewGuid():N}".Substring(0, 10),
                Name = "Ponto Automatizado",
                Neighborhood = "Test",
                MaterialCategory = 0,
                CapacityKg = 500,
                CurrentLoadKg = 100,
                SupportsIoTSensors = true,
                Latitude = -23.5,
                Longitude = -46.6
            })
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);
        return document.RootElement.GetProperty("id").GetGuid();
    }

    private async Task<string> GetTokenAsync(string username, string password)
    {
        var response = await _client.PostAsJsonAsync("api/auth/token", new { Username = username, Password = password });
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);
        return document.RootElement.GetProperty("access_token").GetString()!;
    }
}
