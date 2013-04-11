namespace phiNdus.fundus.Domain.Mails
{
    using System;
    using Entities;
    using Infrastructure;
    using Settings;
    using piNuts.phundus.Infrastructure.Obsolete;

    public class UserChangeEmailValidationMail : BaseMail
    {
        public UserChangeEmailValidationMail()
            : base(Settings.Mail.Templates.UserChangeEmailValidationMail)
        {
        }

        public void Send(User user)
        {
            Send(user.Membership.RequestedEmail);
        }

        public UserChangeEmailValidationMail For(User user)
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
    }
}