using System.Net.Mail;
using System.Threading.Tasks;

namespace Core1.Infrastructure.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(MailAddress sender, MailAddress recipient, string subject, string htmlBody);
    }
}