using Core1.Model;
using Core1.Web.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Core1.Web.Features.Accounts
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
            private readonly IUserContext _userContext;

            public CommandHandler(SignInManager<AppIdentityUser> signInManager, IUserContext userContext)
            {
                _signInManager = signInManager;
                _userContext = userContext;
            }

            public async Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
            {
                await _signInManager.SignOutAsync();
                _userContext.Clear();

                return new CommandResult
                {
                };
            }
        }
    }
}