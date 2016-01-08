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

        [Then(@"anon should receive email ""(.*)"" with text body:")]
        public void DannAnonShouldReceiveEmailWithTextBody(string subject, string textBody)
        {
            AssertEmailReceived(subject, Ctx.AnonEmailAddress, textBody);
        }

        [Then(@"user should receive email ""(.*)""")]
        public void ThenUserShouldReceiveEmail(string subject)
        {
            var toAddress = Ctx.User.EmailAddress;
            AssertEmailReceived(subject, toAddress);
        }

        [Then(@"user should receive email ""(.*)"" at requested address")]
        public void ThenUserShouldReceiveEmailAtRequestedAddress(string subject)
        {
            var toAddress = Ctx.User.RequestedEmailAddress;
            AssertEmailReceived(subject, toAddress);
        }

        [Then(@"""(.*)"" should receive email ""(.*)""")]
        public void ThenShouldReceiveEmail(string toAddress, string subject)
        {
            AssertEmailReceived(subject, toAddress);
        }

        private void AssertEmailReceived(string subject, string toAddress, string textBody = null)
        {
            var message = _mailbox.Find(subject, toAddress);
            Assert.That(message, Is.Not.Null,
                String.Format("Email with subject \"{0}\" to {1} not found.", subject, toAddress));

            if (textBody != null)
                Assert.That(message.TextBody, Is.EqualTo(textBody));
        }}
}