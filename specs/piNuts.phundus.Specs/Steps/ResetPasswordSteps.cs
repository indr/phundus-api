﻿namespace Phundus.Specs.Steps
{
    using System;
    using NUnit.Framework;
    using Services;
    using TechTalk.SpecFlow;

    [Binding]
    public class ResetPasswordSteps : StepsBase
    {
        private readonly IMailbox _mailbox;

        public ResetPasswordSteps(App app, Ctx ctx, IMailbox mailbox) : base(app, ctx)
        {
            if (mailbox == null) throw new ArgumentNullException("mailbox");
            _mailbox = mailbox;
        }

        [When(@"reset password")]
        public void ResetPassword()
        {
            App.ResetPassword(Ctx.CurrentUser.EmailAddress);
        }

        [Then(@"user should receive email ""(.*)""")]
        public void UserShouldReceiveEmail(string subject)
        {
            var toAddress = Ctx.CurrentUser.EmailAddress;
            var message = _mailbox.Find(subject, toAddress);
            Assert.That(message, Is.Not.Null,
                String.Format("Email with subject \"{0}\" to {1} not found.", subject, toAddress));
        }
    }
}