namespace phiNdus.fundus.Domain.Mails
{
    using System;
    using Entities;
    using Infrastructure;
    using Settings;
    using piNuts.phundus.Infrastructure;

    public class UserUnlockedMail : BaseMail
    {
        public UserUnlockedMail()
            : base(Settings.Mail.Templates.UserUnlocked)
        {
        }

        public UserUnlockedMail Send(User user)
        {
            Send(user.Membership.Email);
            return this;
        }

        public UserUnlockedMail For(User user)
        {
            Guard.Against<ArgumentNullException>(user == null, "user");

            Model = new
                {
                    Settings = Settings.GetSettings(),
                    Urls = new Urls(Config.ServerUrl),
                    User = user
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