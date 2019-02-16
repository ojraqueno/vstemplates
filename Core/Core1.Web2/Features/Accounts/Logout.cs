using Core1.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Core1.Web2.Features.Accounts
{
    public class Logout
    {
        public class Command : IRequest<CommandResult>
        {
        }

        public class CommandResult
        {
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly SignInManager<AppIdentityUser> _signInManager;

            public CommandHandler(SignInManager<AppIdentityUser> signInManager)
            {
                _signInManager = signInManager;
            }

            public async Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
            {
                await _signInManager.SignOutAsync();

                return new CommandResult
                {
                };
            }
        }
    }
}