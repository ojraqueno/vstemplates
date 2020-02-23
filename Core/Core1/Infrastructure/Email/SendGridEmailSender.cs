using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Core1.Infrastructure.Email
{
    // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-2.1&tabs=visual-studio#require-email-confirmation
    public class SendGridEmailSender : IEmailSender
    {
        private readonly AuthMessageSenderOptions _options;

        public SendGridEmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public async Task SendEmailAsync(MailAddress sender, MailAddress recipient, string subject, string htmlBody)
        {
            var client = new SendGridClient(_options.SendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(sender.Address, sender.DisplayName),
                Subject = subject,
                PlainTextContent = htmlBody,
                HtmlContent = htmlBody
            };
            msg.AddTo(new EmailAddress(recipient.Address, recipient.DisplayName));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.TrackingSettings = new TrackingSettings
            {
                ClickTracking = new ClickTracking { Enable = false }
            };

            await client.SendEmailAsync(msg);
        }
    }
}