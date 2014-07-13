namespace phiNdus.fundus.Domain.Mails
{
    using System;
    using Entities;
    using Infrastructure;
    using Settings;
    using piNuts.phundus.Infrastructure;

    public class UserAccountCreatedMail : BaseMail
    {
        public UserAccountCreatedMail()
            : base(Settings.Mail.Templates.UserAccountCreated)
        {
        }

        public void Send(User user)
        {
            Send(user.Membership.Email);
        }

        public UserAccountCreatedMail For(User user)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            Model = new
                {
                    Settings = Settings.GetSettings(),
                    Urls = new Urls(Config.ServerUrl),
                    User = user,
                    Admins = Config.FeedbackRecipients
                };
            //DataContext.Add("User", user);
            //DataContext.Add("Membership", user.Membership);
            return this;
        }

        public new void Send(string address)
        {
            base.Send(address);
        }
    }
}