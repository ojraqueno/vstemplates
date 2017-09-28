using System.Collections.Generic;

namespace MVC5_R.WebAPI.Features.Account
{
    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; } = new List<UserLoginInfoViewModel>();

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; } = new List<ExternalLoginViewModel>();
    }
}