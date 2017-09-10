using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC.WebApp.Features.Manage
{
    public class RemoveLogin
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }
}
