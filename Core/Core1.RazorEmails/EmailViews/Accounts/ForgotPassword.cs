using Core1.RazorEmails.Infrastructure;

namespace Core1.RazorEmails.EmailViews.Accounts
{
    public class ForgotPassword : BaseEmailViewModel
    {
        public ForgotPassword(string resetPasswordUrl)
        {
            ResetPasswordUrl = resetPasswordUrl;
        }

        public string ResetPasswordUrl { get; private set; }

        internal override string ViewPath => "/Accounts/ForgotPassword";
    }
}