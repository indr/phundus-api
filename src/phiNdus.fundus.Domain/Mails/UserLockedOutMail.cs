namespace phiNdus.fundus.Domain.Mails
{
    using System;
    using Entities;
    using Infrastructure;
    using Phundus.Infrastructure;
    using Settings;

    public class UserLockedOutMail : BaseMail
    {
        public UserLockedOutMail()
            : base(Settings.Mail.Templates.UserLockedOut)
        {
        }

        public UserLockedOutMail Send(User user)
        {
            Send(user.Membership.Email);
            return this;
        }

        public UserLockedOutMail For(User user)
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