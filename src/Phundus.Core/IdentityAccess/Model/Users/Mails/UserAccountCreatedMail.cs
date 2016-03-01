namespace Phundus.IdentityAccess.Users.Mails
{
    using System;
    using IdentityAccess.Model.Users.Mails;
    using Infrastructure;
    using Infrastructure.Gateways;
    using Model;

    public class UserAccountCreatedMail : BaseMail
    {
        public UserAccountCreatedMail(IMailGateway mailGateway) : base(mailGateway)
        {
        }

        public void Send(User user)
        {
            Send(user.Account.Email, Templates.UserAccountCreatedSubject, null, Templates.UserAccountCreatedHtml);
        }

        public UserAccountCreatedMail For(User user)
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

        public void Send(string address)
        {
            base.Send(address, Templates.UserAccountCreatedSubject, null, Templates.UserAccountCreatedHtml);
        }
    }
}