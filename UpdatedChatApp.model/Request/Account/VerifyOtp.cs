﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatedChatApp.model.Request.Account
{
    public class VerifyOtp
    {
        public string? Email { get; set; }
        public string? OtpCode { get; set; }
    }
}
