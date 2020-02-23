using Core1.Infrastructure.Data;
using Core1.Infrastructure.Email;
using Core1.Model;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace Core1.Web.Features.Accounts
{
    public class ForgotPassword
    {
        public class Command : IRequest<CommandResult>
        {
            public string Email { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            private readonly AppDbContext _db;

            public CommandValidator(AppDbContext db)
            {
                _db = db;

                RuleFor(c => c.Email)
                    .NotEmpty()
                    .DependentRules(() =>
                    {
                        RuleFor(c => c.Email)
                            .EmailAddress();
                    });
            }
        }

        public class CommandResult
        {
            public bool Succeeded { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly IConfiguration _configuration;
            private readonly AppDbContext _db;
            private readonly IEmailSender _emailSender;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUrlHelper _urlHelper;
            private readonly UserManager<AppIdentityUser> _userManager;

            public CommandHandler(IConfiguration configuration, AppDbContext db, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor, IUrlHelper urlHelper, UserManager<AppIdentityUser> userManager)
            {
                _configuration = configuration;
                _db = db;
                _emailSender = emailSender;
                _httpContextAccessor = httpContextAccessor;
                _urlHelper = urlHelper;
                _userManager = userManager;
            }

            public async Task<CommandResult> Handle(Command command, CancellationToken token)
            {
                command.Email = command.Email?.Trim();

                var user = await _userManager.FindByEmailAsync(command.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return new CommandResult { Succeeded = true };
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var resetPasswordUrl = _urlHelper.Action("ResetPassword", "Accounts", values: new { code = code }, protocol: _httpContextAccessor.HttpContext.Request.Scheme);
                if (resetPasswordUrl.Contains("api/accounts/resetPassword"))
                {
                    resetPasswordUrl = resetPasswordUrl.Replace("api/accounts/resetPassword", "Accounts/ResetPassword");
                }

                // Don't wait for finish
                Task.Run(async () =>
                {
                    await _emailSender.SendEmailAsync(
                        new MailAddress(_configuration["Email:ResetPasswordFromEmail"], _configuration["Email:ResetPasswordFromName"]),
                        new MailAddress(command.Email),
                        $"Reset Password",
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(resetPasswordUrl)}'>clicking here</a>. If you did not initiate this action, please ignore this email.");
                });

                return new CommandResult { Succeeded = true };
            }
        }
    }
}