using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MVC5_R.WebAPI.Features.Account
{
    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }
}