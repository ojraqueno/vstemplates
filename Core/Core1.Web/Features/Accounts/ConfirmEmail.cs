using Core1.Infrastructure.Data;
using Core1.Model;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core1.Web.Features.Accounts
{
    public class ConfirmEmail
    {
        public class Command : IRequest<CommandResult>
        {
            public string Code { get; set; }
            public string UserId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            private readonly AppDbContext _db;

            public CommandValidator(AppDbContext db)
            {
                _db = db;
            }
        }

        public class CommandResult
        {
            public bool Succeeded { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly AppDbContext _db;
            private readonly ILogger<ConfirmEmail> _logger;
            private readonly UserManager<AppIdentityUser> _userManager;

            public CommandHandler(AppDbContext db, ILogger<ConfirmEmail> logger, UserManager<AppIdentityUser> userManager)
            {
                _db = db;
                _logger = logger;
                _userManager = userManager;
            }

            public async Task<CommandResult> Handle(Command command, CancellationToken token)
            {
                if (String.IsNullOrWhiteSpace(command.UserId) || String.IsNullOrWhiteSpace(command.Code)) return new CommandResult { Succeeded = false };

                var user = await _userManager.FindByIdAsync(command.UserId);
                if (user == null)
                {
                    _logger.LogWarning($"Tried to confirm email for non-existent user {command.UserId}");
                    return new CommandResult { Succeeded = false };
                }

                var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, command.Code);
                if (!confirmEmailResult.Succeeded)
                {
                    _logger.LogWarning($"Unable to confirm email for user {command.UserId}");
                    return new CommandResult { Succeeded = false };
                }

                return new CommandResult
                {
                    Succeeded = true
                };
            }
        }
    }
}