using Microsoft.AspNetCore.Identity;

namespace QrCodeApp.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ApiKey { get; set; }
    }
}