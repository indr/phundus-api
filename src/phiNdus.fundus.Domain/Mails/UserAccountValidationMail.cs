namespace phiNdus.fundus.Domain.Mails
{
    using System;
    using Entities;
    using Infrastructure;
    using Settings;
    using piNuts.phundus.Infrastructure;

    public class UserAccountValidationMail : BaseMail
    {
        public UserAccountValidationMail() : base(Settings.Mail.Templates.UserAccountValidation)
        {
        }

        public void Send(User user)
        {
            Send(user.Membership.Email);
        }

        public UserAccountValidationMail For(User user)
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