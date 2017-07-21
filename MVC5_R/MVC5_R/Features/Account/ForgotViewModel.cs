using System.ComponentModel.DataAnnotations;

namespace MVC5_R.Features.Account
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}