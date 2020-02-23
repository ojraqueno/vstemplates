using Core1.Infrastructure.Data;
using Core1.Infrastructure.Email;
using Core1.Model;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace Core1.Web.Features.Accounts
{
    public class Register
    {
        public class Command : IRequest<CommandResult>
        {
            public bool AgreedToTerms { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public int? TimezoneOffsetMinutes { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            private readonly AppDbContext _db;
            private readonly UserManager<AppIdentityUser> _userManager;

            public CommandValidator(AppDbContext db, UserManager<AppIdentityUser> userManager)
            {
                _db = db;
                _userManager = userManager;

                RuleFor(c => c.AgreedToTerms)
                    .NotEmpty();

                RuleFor(c => c.Email)
                    .NotEmpty()
                    .DependentRules(() =>
                    {
                        RuleFor(c => c.Email)
                            .EmailAddress();

                        RuleFor(c => c.Email)
                            .MustAsync(BeAValidEmail)
                            .WithMessage("Invalid email.");
                    });

                RuleFor(c => c.Name)
                    .NotEmpty();

                RuleFor(c => c.Password)
                    .NotEmpty()
                    .DependentRules(() =>
                    {
                        RuleFor(c => c.Password)
                            .MustAsync(BeNotTooShort)
                            .WithMessage("Password must be at least 6 characters.")
                            .DependentRules(() =>
                            {
                                RuleFor(c => c.Password)
                                   .MustAsync(HaveANumber)
                                   .WithMessage("Password must have at least one digit ('0'-'9').")
                                   .DependentRules(() =>
                                   {
                                       RuleFor(c => c.Password)
                                           .MustAsync(BeAValidPassword)
                                           .WithMessage("Invalid password.");
                                   });
                            });
                    });
            }

            private async Task<bool> BeNotTooShort(string password, CancellationToken token)
            {
                var identityErrors = new List<IdentityError>();

                foreach (var passwordValidator in _userManager.PasswordValidators)
                {
                    var validatePasswordResult = await passwordValidator.ValidateAsync(_userManager, null, password);
                    identityErrors.AddRange(validatePasswordResult.Errors);
                }

                return !identityErrors.Any(e => e.Code == "PasswordTooShort");
            }

            private async Task<bool> BeAValidEmail(string email, CancellationToken token)
            {
                var identityErrors = new List<IdentityError>();

                foreach (var userValidator in _userManager.UserValidators)
                {
                    var validateUserResult = await userValidator.ValidateAsync(_userManager, new AppIdentityUser { Email = email, UserName = email });
                    identityErrors.AddRange(validateUserResult.Errors);
                }

                return !identityErrors.Any();
            }

            private async Task<bool> BeAValidPassword(string password, CancellationToken token)
            {
                var identityErrors = new List<IdentityError>();

                foreach (var passwordValidator in _userManager.PasswordValidators)
                {
                    var validatePasswordResult = await passwordValidator.ValidateAsync(_userManager, null, password);
                    identityErrors.AddRange(validatePasswordResult.Errors);
                }

                return !identityErrors.Any();
            }

            private async Task<bool> HaveANumber(string password, CancellationToken token)
            {
                var identityErrors = new List<IdentityError>();

                foreach (var passwordValidator in _userManager.PasswordValidators)
                {
                    var validatePasswordResult = await passwordValidator.ValidateAsync(_userManager, null, password);
                    identityErrors.AddRange(validatePasswordResult.Errors);
                }

                return !identityErrors.Any(e => e.Code == "PasswordRequiresDigit");
            }
        }

        public class CommandResult
        {
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

            public async Task<CommandResult> Handle(Command command, CancellationToken cancellationToken)
            {
                command.Email = command.Email?.Trim();
                command.Name = command.Name?.Trim();
                command.Password = command.Password?.Trim();

                var now = DateTime.UtcNow;

                var user = new AppIdentityUser
                {
                    AddedOn = now,
                    Email = command.Email,
                    UserName = command.Email,
                    TimezoneOffsetMinutes = command.TimezoneOffsetMinutes
                };

                var createUserResult = await _userManager.CreateAsync(user, command.Password);
                if (!createUserResult.Succeeded) throw new Exception("Failed to create user!");

                // Don't wait for finish
                Task.Run(async () => await SendConfirmationEmail(command, user));

                return new CommandResult();
            }

            private async Task SendConfirmationEmail(Command command, AppIdentityUser user)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmEmailUrl = _urlHelper.Action("ConfirmEmail", "Accounts", values: new { userId = user.Id, code = code }, protocol: _httpContextAccessor.HttpContext.Request.Scheme);
                if (confirmEmailUrl.Contains("api/accounts/confirmEmail"))
                {
                    confirmEmailUrl = confirmEmailUrl.Replace("api/accounts/confirmEmail", "Accounts/ConfirmEmail");
                }

                await _emailSender.SendEmailAsync(
                    new MailAddress(_configuration["Email:ConfirmEmailFromEmail"], _configuration["Email:ConfirmEmailFromName"]),
                    new MailAddress(command.Email),
                    $"Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmEmailUrl)}'>clicking here</a>.");
            }
        }
    }
}