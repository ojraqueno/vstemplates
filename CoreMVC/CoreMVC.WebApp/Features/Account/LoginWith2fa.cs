﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC.WebApp.Features.Account
{
    public class LoginWith2fa
    {
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
    }
}
