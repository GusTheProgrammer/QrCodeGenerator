using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using QrCodeApp.Shared.Models;
using QrCodeApp.Server.Models;

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
}
