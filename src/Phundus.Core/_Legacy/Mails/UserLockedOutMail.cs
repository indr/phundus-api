﻿namespace Phundus.Core.Mails
{
    using System;
    using Entities;
    using IdentityAndAccessCtx.DomainModel;
    using Infrastructure;
    using Phundus.Infrastructure;

    public class UserLockedOutMail : BaseMail
    {
        public UserLockedOutMail()
            : base(Settings.Settings.Mail.Templates.UserLockedOut)
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
                    Settings = Settings.Settings.GetSettings(),
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