namespace Phundus.Specs.Steps
{
    using System;
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

        [Then(@"anon should receive email ""(.*)""")]
        public void ThenAnonShouldReceiveEmail(string subject)
        {
            AssertEmailReceived(subject, Ctx.AnonEmailAddress);
        }

        [Then(@"user should receive email ""(.*)""")]
        public void ThenUserShouldReceiveEmail(string subject)
        {
            var toAddress = Ctx.CurrentUser.EmailAddress;
            AssertEmailReceived(subject, toAddress);
        }

        [Then(@"""(.*)"" should receive email ""(.*)""")]
        public void ThenShouldReceiveEmail(string toAddress, string subject)
        {
            AssertEmailReceived(subject, toAddress);
        }

        private void AssertEmailReceived(string subject, string toAddress)
        {
            var message = _mailbox.Find(subject, toAddress);
            Assert.That(message, Is.Not.Null,
                String.Format("Email with subject \"{0}\" to {1} not found.", subject, toAddress));
        }
    }
}