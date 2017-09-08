using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MVC5_R.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC5_R.Infrastructure.Identity
{
    // Configure the application sign-in manager which is used in this application.
    public class SignInManager : SignInManager<User, string>
    {
        public SignInManager(UserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static SignInManager Create(IdentityFactoryOptions<SignInManager> options, IOwinContext context)
        {
            return new SignInManager(context.GetUserManager<UserManager>(), context.Authentication);
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((UserManager)UserManager);
        }
    }
}