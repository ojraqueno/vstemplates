﻿using FluentValidation;
using MediatR;
using Microsoft.AspNet.Identity;
using MVC5_R.WebApp.Infrastructure.Identity;
using MVC5_R.WebApp.Infrastructure.Startup;
using System;
using System.Threading.Tasks;

namespace MVC5_R.WebApp.Features.Account
{
    public class ResetPassword
    {
        public class Query : IRequest<Command>
        {
            public string Code { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(c => c.Code)
                    .NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            public Command Handle(Query query)
            {
                return new Command
                {
                    Code = query.Code
                };
            }
        }

        public class Command : IRequest
        {
            public string Code { get; set; }
            public string ConfirmPassword { get; set; }
            public string Email { get; set; }
            public string NewPassword { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            private readonly PasswordValidator _passwordValidator;
            private readonly ApplicationUserManager _userManager;

            public CommandValidator()
            {
                _passwordValidator = ApplicationUserManager.CreatePasswordValidator();
                _userManager = DependencyConfig.Instance.Container.GetInstance<ApplicationUserManager>();

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
        }

        public class CommandHandler : IAsyncRequestHandler<Command>
        {
            private readonly ApplicationUserManager _userManager;

            public CommandHandler(ApplicationUserManager userManager)
            {
                _userManager = userManager;
            }

            public async Task Handle(Command command)
            {
                var user = await _userManager.FindByNameAsync(command.Email);
                if (user == null)
                {
                    // Do nothing: Don't reveal that the user does not exist
                }
                else
                {
                    var result = await _userManager.ResetPasswordAsync(user.Id, command.Code, command.NewPassword);
                    if (!result.Succeeded)
                    {
                        throw new Exception("Unable to reset password. The link may have expired.");
                    }
                }
            }
        }
    }
}