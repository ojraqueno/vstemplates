using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC.WebApp.Features.Account
{
    public class ExternalLogin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
