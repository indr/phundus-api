namespace Phundus.Core.IdentityAndAccess.Users.Mails
{
    using System;
    using Infrastructure;
    using Model;

    public class UserChangeEmailValidationMail : BaseMail
    {
        public void Send(User user)
        {
            Send(user.Account.RequestedEmail, Templates.UserChangeEmailValidationSubject, null,
                Templates.UserChangeEmailValidationHtml);
        }

        public UserChangeEmailValidationMail For(User user)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            Model = new
            {
                Urls = new Urls(Config.ServerUrl),
                User = user,
                Admins = Config.FeedbackRecipients
            };

            return this;
        }
    }
}