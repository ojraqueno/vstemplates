﻿using MediatR;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MVC5_R.Infrastructure.Identity;
using FluentValidation;
using MVC5_R.WebApp.Infrastructure.Startup;
using System.ComponentModel.DataAnnotations;

namespace MVC5_R.WebApp.Features.Account
{
    public class VerifyCode
    {
        public class Query : IRequest<Command>
        {
            public string Provider { get; set; }
            public bool RememberMe { get; set; }
            public string ReturnUrl { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Command>
        {
            private readonly SignInManager _signInManager;

            public QueryHandler(SignInManager signInManager)
            {
                _signInManager = signInManager;
            }

            public async Task<Command> Handle(Query query)
            {
                // Require that the user has already logged in via username/password or external login
                if (!await _signInManager.HasBeenVerifiedAsync())
                {
                    throw new Exception("User not verified.");
                }

                return new Command
                {
                    Provider = query.Provider,
                    RememberMe = query.RememberMe,
                    ReturnUrl = query.ReturnUrl
                };
            }
        }

        public class Command : IRequest<SignInStatus>
        {
            public string Code { get; set; }            
            public string Provider { get; set; }
            [Display(Name = "Remember this browser?")]
            public bool RememberBrowser { get; set; }
            public bool RememberMe { get; set; }
            public string ReturnUrl { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Code)
                    .NotEmpty();

                RuleFor(c => c.Provider)
                    .NotEmpty();
            }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, SignInStatus>
        {
            private readonly SignInManager _signInManager;

            public CommandHandler(SignInManager signInManager)
            {
                _signInManager = signInManager;
            }

            public async Task<SignInStatus> Handle(Command command)
            {
                // The following code protects for brute force attacks against the two factor codes.
                // If a user enters incorrect codes for a specified amount of time then the user account
                // will be locked out for a specified amount of time.
                // You can configure the account lockout settings in IdentityConfig
                return await _signInManager.TwoFactorSignInAsync(command.Provider, command.Code, isPersistent: command.RememberMe, rememberBrowser: command.RememberBrowser);
            }
        }
    }
}