using System.Collections.Generic;
using System.Web.Mvc;

namespace MVC5_R.WebApp.Features.Account
{
    public class SendCodeViewModel
    {
        public ICollection<SelectListItem> Providers { get; set; } = new List<SelectListItem>();
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
        public string SelectedProvider { get; set; }
    }
}