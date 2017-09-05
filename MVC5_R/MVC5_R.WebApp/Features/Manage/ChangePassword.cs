using MediatR;
using System;
using System.Web;
using System.Threading.Tasks;
using MVC5_R.WebApp.Infrastructure.Identity;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using System.Security.Principal;
using MVC5_R.WebApp.Infrastructure.Startup;

namespace MVC5_R.WebApp.Features.Manage
{
    public class ChangePassword
    {
        public class Command : IRequest
        {
            [Display(Name = "Confirm new password")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [Display(Name = "Current password")]
            public string OldPassword { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            private readonly PasswordValidator _passwordValidator;
            private readonly ApplicationUserManager _userManager;

            public Validator()
            {
                _passwordValidator = ApplicationUserManager.CreatePasswordValidator();
                _userManager = DependencyConfig.Instance.Container.GetInstance<ApplicationUserManager>();

                RuleFor(c => c.OldPassword)
                    .NotEmpty();

                RuleFor(c => c.OldPassword)
                    .Must(IsValidPasswordForUser)
                    .WithMessage("Old password is invalid.");

                RuleFor(c => c.NewPassword)
                    .NotEmpty();

                RuleFor(c => c.NewPassword)
                    .Must(c => _passwordValidator.ValidateAsync(c).Result.Succeeded)
                    .WithMessage(c => String.Join(",", _passwordValidator.ValidateAsync(c.NewPassword).Result.Errors));

                RuleFor(c => c.ConfirmPassword)
                    .NotEmpty();

                RuleFor(c => c.ConfirmPassword)
                    .Must(IsNewPasswordAndConfirmPasswordTheSame)
                    .WithMessage("The new password and confirm password values must match.");
            }

            private bool IsNewPasswordAndConfirmPasswordTheSame(Command command, string confirmPassword)
            {
                return String.Equals(command.NewPassword, confirmPassword);
            }

            private bool IsValidPasswordForUser(string oldPassword)
            {
                var currentUser = HttpContext.Current.User.Identity.GetCurrentUser();
                return _userManager.CheckPassword(currentUser, oldPassword);
            }
        }

        public class Handler : IAsyncRequestHandler<Command>
        {
            private readonly ApplicationSignInManager _signInManager;
            private readonly ApplicationUserManager _userManager;

            public Handler(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task Handle(Command command)
            {
                var currentUser = HttpContext.Current.User.Identity.GetCurrentUser();
                var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser.Id, command.OldPassword, command.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    throw new Exception($"Unable to change password. Errors: {String.Join(",", changePasswordResult.Errors)}");
                }

                var user = await _userManager.FindByIdAsync(currentUser.Id);
                if (user != null)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
            }
        }
    }
}