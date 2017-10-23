using Microsoft.AspNet.Identity;
using MVC5_R.Infrastructure.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace MVC5_R.Infrastructure.Email
{
    // More info: https://docs.microsoft.com/en-us/azure/app-service-web/sendgrid-dotnet-how-to-send-email
    public class SendGridEmailService : IIdentityMessageService
    {
        private readonly SendGridClient _client;

        public SendGridEmailService()
        {
            var apiKey = AppSettings.String("SendGridApiKey");
            _client = new SendGridClient(apiKey);
        }

        public async Task SendAsync(IdentityMessage message)
        {
            var sendGridMessage = new SendGridMessage();

            sendGridMessage.SetFrom(new EmailAddress(EmailAddresses.NoReply, $"{AppSettings.String("ApplicationName")} Team"));
            sendGridMessage.AddTo(message.Destination);
            sendGridMessage.SetSubject(message.Subject);
            sendGridMessage.AddContent(MimeType.Html, message.Body);

            await _client.SendEmailAsync(sendGridMessage);
        }
    }
}