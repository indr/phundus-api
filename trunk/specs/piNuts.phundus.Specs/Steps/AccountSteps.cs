namespace Phundus.Specs.Steps
{
    using System;
    using NUnit.Framework;
    using Services;
    using TechTalk.SpecFlow;

    [Binding]
    public class AccountSteps : StepsBase
    {
        private readonly RestMailbox _mailbox;

        public AccountSteps(App app, Ctx ctx, RestMailbox mailbox) : base(app, ctx)
        {
            _mailbox = mailbox;
            if (mailbox == null) throw new ArgumentNullException("mailbox");
        }

        [When(@"Passwort zurücksetzen")]
        public void WennPasswortZurucksetzen()
        {
            App.ResetPassword(Ctx.User.EmailAddress);
        }

        [Then(@"E-Mail ""(.*)"" an Benutzer")]
        public void DannE_MailAnBenutzer(string subject)
        {
            var toAddress = Ctx.User.EmailAddress;
            var message = _mailbox.Find(subject, toAddress);
            Assert.That(message, Is.Not.Null, String.Format("E-Mail mit Betreff \"{0}\" an {1} nicht gefunden.", subject, toAddress));
        }
    }
}