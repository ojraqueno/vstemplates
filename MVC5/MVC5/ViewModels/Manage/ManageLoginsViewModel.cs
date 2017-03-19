using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;

namespace MVC5.ViewModels.Manage
{
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; } = new List<UserLoginInfo>();
        public IList<AuthenticationDescription> OtherLogins { get; set; } = new List<AuthenticationDescription>();
    }
}