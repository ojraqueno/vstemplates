using System.ComponentModel.DataAnnotations;

namespace MVC5_R.WebApp.Features.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}