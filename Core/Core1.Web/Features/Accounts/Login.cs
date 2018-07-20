using Core1.Infrastructure.Data;
using Core1.Model;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core1.Web.Features.Accounts
{
    public class Login
    {
        public class Command : IRequest<CommandResult>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
            public string ReturnUrl { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            private readonly SignInManager<AppIdentityUser> _signInManager;
            private readonly UserManager<AppIdentityUser> _userManager;

            public CommandValidator(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager)
            {
                _signInManager = signInManager;
                _userManager = userManager;

                RuleFor(c => c.Email)
                    .NotEmpty();

                RuleFor(c => c.Password)
                    .NotEmpty();

                When(c => !String.IsNullOrWhiteSpace(c.Email) && !String.IsNullOrWhiteSpace(c.Password), () =>
                {
                    RuleFor(c => c.Email)
                        .MustAsync(HaveCorrectPassword)
                        .WithMessage("Invalid login attempt.");
                });
            }

            private async Task<bool> HaveCorrectPassword(Command command, string email, CancellationToken token)
            {
                var user = await _userManager.FindByNameAsync(email);
                var checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, command.Password, false);

                return checkPasswordResult.Succeeded;
            }
        }

        public class CommandResult
        {
            public string ReturnUrl { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly AppDbContext _db;
            private readonly SignInManager<AppIdentityUser> _signInManager;

            public CommandHandler(AppDbContext db, SignInManager<AppIdentityUser> signInManager)
            {
                _db = db;
                _signInManager = signInManager;
            }

            public async Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
            {
                var result = await _signInManager.PasswordSignInAsync(command.Email, command.Password, command.RememberMe, lockoutOnFailure: true);
                if (!result.Succeeded) throw new Exception($"Login exception for user {command.Email}");

                return new CommandResult
                {
                    ReturnUrl = String.IsNullOrWhiteSpace(command.ReturnUrl) ? "/Home/Index" : command.ReturnUrl
                };
            }
        }
    }
}