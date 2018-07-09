using AutoMapper;
using Core.Infrastructure.Data;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Core.Model;

namespace Core.Web.Features.Accounts
{
    public class Login
    {
        public class Command : IRequest<CommandResult>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(AppDbContext db)
            {
            }
        }

        public class CommandResult
        {
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly AppDbContext _db;
            private readonly SignInManager<AppIdentityUser> _signInManager;
            private readonly UserManager<AppIdentityUser> _userManager;

            public CommandHandler(AppDbContext db, SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager)
            {
                _db = db;
                _signInManager = signInManager;
                _userManager = userManager;
            }

            public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
            {
                return new CommandResult();
            }
        }
    }
}
