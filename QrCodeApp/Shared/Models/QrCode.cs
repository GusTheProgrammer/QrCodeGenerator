using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrCodeApp.Shared.Models
{
   public class QrCodeResponse
    {
        public string title { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string options { get; set; }
        public string type { get; set; }
        public string reference { get; set; }
        public string qrCode { get; set; }
    }

    public class QrCodeData
    {
        public string msg { get; set; }
        public List<QrCodeResponse> data { get; set; }
    }

    public class QRCodeRequest
    {
        public string title { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string type { get; set; }

    }
}
