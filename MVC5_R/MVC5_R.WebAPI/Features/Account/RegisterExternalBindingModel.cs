using System.ComponentModel.DataAnnotations;

namespace MVC5_R.WebAPI.Features.Account
{
    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}