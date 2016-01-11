﻿namespace Phundus.Specs.Steps
{
    using System;
    using System.Text.RegularExpressions;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using TechTalk.SpecFlow;

    [Binding]
    public class EmailSteps : StepsBase
    {
        private readonly IMailbox _mailbox;

        public EmailSteps(App app, Ctx ctx, IMailbox mailbox)
            : base(app, ctx)
        {
            if (mailbox == null) throw new ArgumentNullException("mailbox");
            _mailbox = mailbox;
        }

        [Given(@"I got the validation key from account validation email")]
        public void GivenIGotTheValidationKeyFromAccountValidationEmail()
        {
            var mail = AssertEmailReceived("[phundus] Validierung der E-Mail-Adresse", Ctx.User.EmailAddress);
            var match = new Regex(@"\/#\/validate\/account\?key=([a-z0-9]{24})<").Match(mail.HtmlBody);
            Assert.IsTrue(match.Success, "Could not find validation key in account validation email.");
            Ctx.ValidationKey = match.Groups[1].Value;
        }

        [Given(@"I got the validation key from email validation email")]
        public void GivenIGotTheValidationKeyFromEmailValidationEmail()
        {
            var mail = AssertEmailReceived("[phundus] Validierung der geänderten E-Mail-Adresse",
                Ctx.User.RequestedEmailAddress);
            var match = new Regex(@"\/#\/validate\/email-address\?key=([a-z0-9]{24})<").Match(mail.HtmlBody);
            Assert.IsTrue(match.Success, "Could not find validation key in email validation email.");
            Ctx.ValidationKey = match.Groups[1].Value;
        }

        [Then(@"anon should receive email ""(.*)"" with text body:")]
        public void ThenAnonShouldReceiveEmailWithTextBody(string subject, string textBody)
        {
            AssertEmailReceived(subject, Ctx.AnonEmailAddress, textBody);
        }

        [Then(@"I should receive an email ""(.*)"" at requested address")]
        public void ThenIShouldReceiveAnEmailAtRequestedAddress(string subject)
        {
            var toAddress = Ctx.User.RequestedEmailAddress;
            AssertEmailReceived(subject, toAddress);
        }

        [Then(@"I should receive email ""(.*)""")]
        public void ThenIShouldReceiveEmail(string subject)
        {
            var toAddress = Ctx.User.EmailAddress;
            AssertEmailReceived(subject, toAddress);
        }

        [Then(@"user should receive email ""(.*)""")]
        public void ThenUserShouldReceiveEmail(string subject)
        {
            var toAddress = Ctx.User.EmailAddress;
            AssertEmailReceived(subject, toAddress);
        }

        [Then(@"""(.*)"" should receive email ""(.*)""")]
        public void ThenShouldReceiveEmail(string toAddress, string subject)
        {
            AssertEmailReceived(subject, toAddress);
        }

        private Mail AssertEmailReceived(string subject, string toAddress, string textBody = null)
        {
            var mail = _mailbox.Find(subject, toAddress);
            Assert.That(mail, Is.Not.Null,
                String.Format("Email with subject \"{0}\" to {1} not found.", subject, toAddress));

            if (textBody != null)
                Assert.That(mail.TextBody, Is.EqualTo(textBody));
            return mail;
        }
    }
}