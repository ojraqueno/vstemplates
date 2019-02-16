using Core1.Infrastructure.Data;
using Core1.Model;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Core1.Web2.Features.Accounts
{
    public class ResetPassword
    {
        public class Query : IRequest<QueryResult>
        {
            public string Code { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            private readonly AppDbContext _db;

            public QueryValidator(AppDbContext db)
            {
                _db = db;
            }
        }

        public class QueryResult
        {
            public string Code { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, QueryResult>
        {
            private readonly AppDbContext _db;

            public QueryHandler(AppDbContext db)
            {
                _db = db;
            }

            public async Task<QueryResult> Handle(Query query, CancellationToken token)
            {
                return new QueryResult
                {
                    Code = query.Code
                };
            }
        }

        public class Command : IRequest<CommandResult>
        {
            public string Code { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            private readonly AppDbContext _db;

            public CommandValidator(AppDbContext db)
            {
                _db = db;

                RuleFor(c => c.Code)
                    .NotEmpty();

                RuleFor(c => c.Email)
                    .NotEmpty()
                    .DependentRules(() =>
                    {
                        RuleFor(c => c.Email)
                            .EmailAddress();
                    });

                RuleFor(c => c.Password)
                    .NotEmpty();
            }
        }

        public class CommandResult
        {
            public bool Succeeded { get; set; }
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

            public async Task<CommandResult> Handle(Command command, CancellationToken token)
            {
                command.Code = command.Code?.Trim();
                command.Email = command.Email?.Trim();
                command.Password = command.Password?.Trim();

                var user = await _userManager.FindByEmailAsync(command.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return new CommandResult { Succeeded = true };
                }

                var result = await _userManager.ResetPasswordAsync(user, command.Code, command.Password);
                if (result.Succeeded)
                {
                    return new CommandResult { Succeeded = true };
                }

                return new CommandResult { Succeeded = false };
            }
        }
    }
}