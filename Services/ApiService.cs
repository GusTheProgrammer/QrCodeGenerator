using System.Net.Http.Headers;
using QrCodeGenerator.Models;
using Microsoft.JSInterop;
using QrCodeGenerator.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly GlobalStateService _globalStateService;

    public ApiService(HttpClient httpClient, IJSRuntime jsRuntime, GlobalStateService globalStateService)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        _globalStateService = globalStateService;
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
            return body; 
        }
    }

    public async Task<QrCodeData> GetQRCodes()
    {
        var apiKey = await GetApiKeyFromLocalStorage();

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
        return System.Text.Json.JsonSerializer.Deserialize<QrCodeData>(responseBody);
    }

    public async Task<string> CreateQRCode(QRCodeRequest qrCodeRequest)
    {
        var apiKey = await GetApiKeyFromLocalStorage();

        var requestUri = qrCodeRequest.type.ToLower() == "dynamic"
            ? "https://qr-code-dynamic-and-static1.p.rapidapi.com/qrcode/dynamic"
            : "https://qr-code-dynamic-and-static1.p.rapidapi.com/qrcode/static";

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(requestUri),
            Headers =
        {
            { "api-key", apiKey },
            { "X-RapidAPI-Key", "551508ce8amsh3073d2efcf85c6cp165dd6jsn01c2166ee7f2" },
            { "X-RapidAPI-Host", "qr-code-dynamic-and-static1.p.rapidapi.com" },
        },
            Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(qrCodeRequest))
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            }
        };

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }

    public async Task<string> UpdateQRCode(string id, QRCodeRequest qrCodeRequest)
    {
        var apiKey = await GetApiKeyFromLocalStorage();
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("API key not found.");
        }

        var requestUri = qrCodeRequest.type.ToLower() == "dynamic"
            ? $"https://qr-code-dynamic-and-static1.p.rapidapi.com/qrcode/dynamic/{id}"
            : $"https://qr-code-dynamic-and-static1.p.rapidapi.com/qrcode/static/{id}";

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Patch,
            RequestUri = new Uri(requestUri),
            Headers =
        {
            { "api-key", apiKey },
            { "X-RapidAPI-Key", "551508ce8amsh3073d2efcf85c6cp165dd6jsn01c2166ee7f2" },
            { "X-RapidAPI-Host", "qr-code-dynamic-and-static1.p.rapidapi.com" },
        },
            Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(qrCodeRequest))
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            }
        };

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }

    public async Task<string> DeleteQRCode(string id)
    {
        var apiKey = await GetApiKeyFromLocalStorage();
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("API key not found.");
        }

        var requestUri = $"https://qr-code-dynamic-and-static1.p.rapidapi.com/qrcode/{id}";

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(requestUri),
            Headers =
        {
            { "api-key", apiKey },
            { "X-RapidAPI-Key", "551508ce8amsh3073d2efcf85c6cp165dd6jsn01c2166ee7f2" },
            { "X-RapidAPI-Host", "qr-code-dynamic-and-static1.p.rapidapi.com" },
        },
        };

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return body;
    }



    private async Task<string> GetApiKeyFromLocalStorage()
    {
        var apiKey = await _globalStateService.GetApiKeyAsync();
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("API key not found.");
        }
        return apiKey;
    }




}
