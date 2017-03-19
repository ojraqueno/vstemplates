using System.Collections.Generic;
using System.Web.Mvc;

namespace MVC5.ViewModels.Manage
{
    public class ConfigureTwoFactorViewModel
    {
        public ICollection<SelectListItem> Providers { get; set; } = new List<SelectListItem>();
        public string SelectedProvider { get; set; }
    }
}