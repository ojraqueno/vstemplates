using FluentValidation;
using MediatR;
using MVC5_R.WebApp.Controllers;
using MVC5_R.WebApp.Infrastructure.Identity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC5_R.WebApp.Features.Account
{
    public class ForgotPassword
    {
        public class Command : IRequest
        {
            public string Email { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Email)
                    .NotEmpty();

                RuleFor(c => c.Email)
                    .EmailAddress();
            }
        }

        public class Handler : IAsyncRequestHandler<Command>
        {
            private readonly ApplicationUserManager _userManager;

            public Handler(ApplicationUserManager applicationUserManager)
            {
                _userManager = applicationUserManager;
            }

            public async Task Handle(Command model)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Do nothing: Don't reveal that the user does not exist or is not confirmed
                }
                else
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                    string code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = urlHelper.Action(nameof(AccountController.ResetPassword), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Current.Request.Url.Scheme);

                    // No await means fire and forget
                    _userManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>. If you did not request for a password reset, no action is required.");
                }
            }
        }
    }
}