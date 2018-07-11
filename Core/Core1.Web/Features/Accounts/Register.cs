using Core1.Infrastructure.Data;
using Core1.Model;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core1.Web.Features.Accounts
{
    public class Register
    {
        public class Command : IRequest<CommandResult>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            private readonly AppDbContext _db;
            private readonly UserManager<AppIdentityUser> _userManager;

            public CommandValidator(AppDbContext db, UserManager<AppIdentityUser> userManager)
            {
                _db = db;
                _userManager = userManager;

                RuleFor(c => c.Email)
                    .NotEmpty()
                    .DependentRules(() =>
                    {
                        RuleFor(c => c.Email)
                            .EmailAddress();

                        RuleFor(c => c.Email)
                            .MustAsync(BeAValidEmail)
                            .WithMessage("Invalid email.");
                    });

                RuleFor(c => c.Password)
                    .NotEmpty()
                    .DependentRules(() =>
                    {
                        RuleFor(c => c.Password)
                            .MustAsync(BeAValidPassword)
                            .WithMessage("Invalid password.");
                    });
            }

            private async Task<bool> BeAValidEmail(string email, CancellationToken arg2)
            {
                var identityErrors = new List<IdentityError>();

                foreach (var userValidator in _userManager.UserValidators)
                {
                    var validateUserResult = await userValidator.ValidateAsync(_userManager, new AppIdentityUser { Email = email, UserName = email });
                    identityErrors.AddRange(validateUserResult.Errors);
                }

                return !identityErrors.Any();
            }

            private async Task<bool> BeAValidPassword(string password, CancellationToken arg2)
            {
                var identityErrors = new List<IdentityError>();

                foreach (var passwordValidator in _userManager.PasswordValidators)
                {
                    var validatePasswordResult = await passwordValidator.ValidateAsync(_userManager, null, password);
                    identityErrors.AddRange(validatePasswordResult.Errors);
                }

                return !identityErrors.Any();
            }
        }

        public class CommandResult
        {
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly AppDbContext _db;
            private readonly UserManager<AppIdentityUser> _userManager;

            public CommandHandler(AppDbContext db, UserManager<AppIdentityUser> userManager)
            {
                _db = db;
                _userManager = userManager;
            }

            public async Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
            {
                var user = new AppIdentityUser { UserName = command.Email, Email = command.Email };

                var createUserResult = await _userManager.CreateAsync(user, command.Password);
                if (!createUserResult.Succeeded) throw new Exception("Failed to create user!");

                //await SendConfirmationEmail(command, user);

                return new CommandResult();
            }

            //private async Task SendConfirmationEmail(Command command, AppIdentityUser user)
            //{
            //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            //    var confirmEmailUrl = _urlHelper.Action("ConfirmEmail", "Accounts", values: new { userId = user.Id, code = code }, protocol: _httpContextAccessor.HttpContext.Request.Scheme);

            //    await _emailSender.SendEmailAsync(command.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmEmailUrl)}'>clicking here</a>.");
            //}
        }
    }
}