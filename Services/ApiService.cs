using QrCodeGenerator.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using QrCodeGenerator.Models;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Net.Http.Headers;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ApiServiceConfig _config;
    private readonly IJSRuntime _jsRuntime;

    public ApiService(HttpClient httpClient, ApiServiceConfig apiServiceConfig, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _config = apiServiceConfig;
        _jsRuntime = jsRuntime;
    }


    public async Task<string> GenerateApiKey()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://qr-code-dynamic-and-static1.p.rapidapi.com/user/generate"),
            Headers =
            {
                { "X-RapidAPI-Key", "551508ce8amsh3073d2efcf85c6cp165dd6jsn01c2166ee7f2" },
                { "X-RapidAPI-Host", "qr-code-dynamic-and-static1.p.rapidapi.com" },
            },
        };

        using (var response = await _httpClient.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);
            return body; // Assuming the API key is in the response body
        }
    }

    public async Task<QrCodeData> GetQRCodes()
    {
        var userJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "user");
        if (string.IsNullOrEmpty(userJson))
        {
            throw new InvalidOperationException("User data not found in local storage.");
        }

        var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(userJson);
        var apiKey = user.ApiKey;
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("API key not found in user data.");
        }

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://qr-code-dynamic-and-static1.p.rapidapi.com/qrcode"),
            Headers =
            {
                { "api-key", apiKey },
                { "X-RapidAPI-Key", "551508ce8amsh3073d2efcf85c6cp165dd6jsn01c2166ee7f2" },
                { "X-RapidAPI-Host", "qr-code-dynamic-and-static1.p.rapidapi.com" },
            },
        };

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody);
        return System.Text.Json.JsonSerializer.Deserialize<QrCodeData>(responseBody);
    }

}
