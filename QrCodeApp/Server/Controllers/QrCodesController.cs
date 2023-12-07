using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using QrCodeApp.Shared.Models;
using QrCodeApp.Server.Models;
using static QrCodeApp.Client.Pages.Index;

[ApiController]
[Route("[controller]")]
[Authorize]
public class QRCodeController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly UserManager<ApplicationUser> _userManager;


    public QRCodeController(IHttpClientFactory clientFactory, UserManager<ApplicationUser> userManager)
    {
        _clientFactory = clientFactory;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<QrCodeData>> GetQRCodes()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var apiKey = user.ApiKey;
        Console.WriteLine(apiKey);
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://qr-code-dynamic-and-static1.p.rapidapi.com/qrcode"),
            Headers =
            {
                { "api-key", apiKey },  
                { "X-RapidAPI-Key", "TOKEN" },
                { "X-RapidAPI-Host", "qr-code-dynamic-and-static1.p.rapidapi.com" },
            },
        };

        var client = _clientFactory.CreateClient();
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var qrCodeResponse = JsonConvert.DeserializeObject<QrCodeData>(responseBody);
            return Ok(qrCodeResponse);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateQRCode([FromBody] QRCodeRequest qrCodeRequest)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var apiKey = user.ApiKey;  // Fetch API key from UserManager
        var client = _clientFactory.CreateClient();
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
                { "X-RapidAPI-Key", "TOKEN" },
                { "X-RapidAPI-Host", "qr-code-dynamic-and-static1.p.rapidapi.com" },
            },
            Content = new StringContent(JsonConvert.SerializeObject(qrCodeRequest))

            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            }
        };

        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return Ok(body);
    }

}
