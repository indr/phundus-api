namespace Phundus.IdentityAccess.Users.Mails
{
    using System;
    using Common;
    using Common.Mailing;
    using IdentityAccess.Model.Users.Mails;    
    using Model;

    public class UserAccountCreatedMail : BaseMail
    {
        public UserAccountCreatedMail(IMailGateway mailGateway) : base(mailGateway)
        {
        }

        public void Send(DateTime date, User user)
        {
            Send(date, user.Account.Email, Templates.UserAccountCreatedSubject, null, Templates.UserAccountCreatedHtml);
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

        public void Send(DateTime date, string address)
        {
            base.Send(date, address, Templates.UserAccountCreatedSubject, null, Templates.UserAccountCreatedHtml);
        }
    }
}