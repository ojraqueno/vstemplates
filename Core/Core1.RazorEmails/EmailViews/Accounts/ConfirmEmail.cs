using Enketo.RazorEmails.Infrastructure;

namespace Enketo.RazorEmails.EmailViews.Accounts
{
    public class ConfirmEmail : BaseEmailViewModel
    {
        public ConfirmEmail(string confirmEmailUrl, string name)
        {
            ConfirmEmailUrl = confirmEmailUrl;
            Name = name;
        }

        public string ConfirmEmailUrl { get; private set; }
        public string Name { get; private set; }

        internal override string ViewPath => "/Accounts/ConfirmEmail";
    }
}