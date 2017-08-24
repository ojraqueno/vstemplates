﻿using MVC5_R.Controllers;
using MVC5_R.Data;
using MVC5_R.Infrastructure.Identity;
using MVC5_R.Infrastructure;
using MVC5_R.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MVC5_R.Infrastructure.Configuration;

namespace MVC5_R.Features.Account
{
    public class Register
    {
        public class Command : IRequest
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            private readonly PasswordValidator _passwordValidator = ApplicationUserManager.CreatePasswordValidator();

            public Validator()
            {
                RuleFor(c => c.Email)
                    .NotEmpty();

                RuleFor(c => c.Email)
                    .EmailAddress();

                RuleFor(c => c.Name)
                    .NotEmpty();

                RuleFor(c => c.Password)
                    .NotEmpty();

                RuleFor(c => c.Password)
                    .Must(c => _passwordValidator.ValidateAsync(c).Result.Succeeded)
                    .WithMessage(c => String.Join(",", _passwordValidator.ValidateAsync(c.Password).Result.Errors));
            }

            private bool IsPasswordValid(string password)
            {
                var passwordValidator = ApplicationUserManager.CreatePasswordValidator();
                return passwordValidator.ValidateAsync(password).Result.Succeeded;
            }
        }

        public class Handler : IAsyncRequestHandler<Command>
        {
            private readonly ApplicationDbContext _db;
            private readonly ApplicationUserManager _userManager;
            private readonly ApplicationSignInManager _signInManager;

            public Handler(ApplicationDbContext db, ApplicationUserManager userManager, ApplicationSignInManager signInManager)
            {
                _db = db;
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task Handle(Command command)
            {
                var user = new ApplicationUser { Email = command.Email, UserName = command.Email, };
                var createUserResult = await _userManager.CreateAsync(user, command.Password);
                if (!createUserResult.Succeeded)
                {
                    throw new Exception($"Unable to create user. Errors: {String.Join(",", createUserResult.Errors)}");
                }

                await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = urlHelper.Action(nameof(AccountController.ConfirmEmail), "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Current.Request.Url.Scheme);
                await _userManager.SendEmailAsync(user.Id, "Confirm your email", $"Welcome to {AppSettings.String("ApplicationName")}! Please confirm your email by clicking <a href=\"" + callbackUrl + "\">here</a>.");
            }
        }
    }
}