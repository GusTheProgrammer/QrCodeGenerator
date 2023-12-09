﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrCodeApp.Shared.Models;
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
    public string? Id { get; set; }

    [Required]
    public string title { get; set; }
    [Required]
    public string description { get; set; }
    [Required]
    public string link { get; set; }
    [Required]
    public string type { get; set; }

}


public class QrCodeActionResponse
{
    public string msg { get; set; }
    public string @ref { get; set; }
    public string updated { get; set; }

}


