using Core1.Infrastructure.Data;
using Core1.Model;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core1.Web2.Features.Accounts
{
    public class CreateToken
    {
        public class Command : IRequest<CommandResult>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            private readonly AppDbContext _db;
            private readonly SignInManager<AppIdentityUser> _signInManager;
            private readonly UserManager<AppIdentityUser> _userManager;

            public CommandValidator(AppDbContext db, SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager)
            {
                _db = db;
                _signInManager = signInManager;
                _userManager = userManager;

                RuleFor(c => c.Username)
                    .NotEmpty()
                    .DependentRules(() =>
                    {
                        RuleFor(c => c.Username)
                            .MustAsync(Exist)
                            .WithMessage("Invalid username or password.")
                            .DependentRules(() =>
                            {
                                RuleFor(c => c.Password)
                                    .MustAsync(BeCorrectPassword)
                                    .WithMessage("Invalid username or password.");
                            });
                    });

                RuleFor(c => c.Password)
                    .NotEmpty();
            }

            private async Task<bool> BeCorrectPassword(Command command, string password, CancellationToken token)
            {
                var user = await _userManager.FindByNameAsync(command.Username);
                var checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);

                return checkPasswordResult.Succeeded;
            }

            private async Task<bool> Exist(string username, CancellationToken token)
            {
                var user = await _userManager.FindByNameAsync(username);

                return user != null;
            }
        }

        public class CommandResult
        {
            public string Token { get; set; }
            public DateTime Expiration { get; set; }
            public bool Succeeded { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly AppDbContext _db;
            private readonly IConfiguration _config;
            private readonly SignInManager<AppIdentityUser> _signInManager;
            private readonly UserManager<AppIdentityUser> _userManager;

            public CommandHandler(AppDbContext db, IConfiguration config, SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager)
            {
                _db = db;
                _config = config;
                _signInManager = signInManager;
                _userManager = userManager;
            }

            public async Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
            {
                command.Username = command.Username?.Trim();
                command.Password = command.Password?.Trim();

                var commandResult = new CommandResult();

                var user = await _userManager.FindByNameAsync(command.Username);

                var checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, command.Password, false);
                commandResult.Succeeded = checkPasswordResult.Succeeded;
                if (checkPasswordResult.Succeeded)
                {
                    var token = CreateJwtSecurityToken(user);

                    commandResult.Token = new JwtSecurityTokenHandler().WriteToken(token);
                    commandResult.Expiration = token.ValidTo;
                }

                return commandResult;
            }

            private JwtSecurityToken CreateJwtSecurityToken(AppIdentityUser user)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Security:Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _config["Security:Tokens:Issuer"],
                    _config["Security:Tokens:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddDays(14),
                    signingCredentials: creds);

                return token;
            }
        }
    }
}