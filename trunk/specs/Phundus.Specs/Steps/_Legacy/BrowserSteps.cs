namespace Phundus.Specs.Steps
{
    using System;
    using System.Configuration;
    using System.Reflection;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using TechTalk.SpecFlow;
    using WatiN.Core;

    [Binding]
    public class BrowserSteps : StepBase
    {
        private readonly IMailbox _mailbox;

        public BrowserSteps(IMailbox mailbox)
        {
            _mailbox = mailbox;
            if (mailbox == null) throw new ArgumentNullException("mailbox");
        }

       
        [Given(@"Ich bin auf der Startseite")]
        public void GivenIchBinAufDerStartseite()
        {
            Browser.GoTo(BaseUrl + "/shop");
        }


        [Given(@"ich bin nicht angemeldet")]
        public void GivenIchBinNichtAngemeldet()
        {
            if (Browser.Url == "about:blank")
                return;

            var link = Browser.Link(Find.ByClass("logOffTag"));

            if (!link.Exists)
                return;
            Browser.GoTo(link.Url);
        }

        [Given(@"ich bin als Benutzer angemeldet")]
        public void GivenIchBinAlsBenutzerAngemeldet()
        {
            Login("user@test.phundus.ch", "1234");
        }

        [Then(@"muss ""(.*)"" ein E-Mail erhalten mit dem Betreff ""(.*)""")]
        public Mail DannMussEinEMailErhaltenMitDemBetreff(string adresse, string betreff)
        {
            var message = _mailbox.Find(betreff, adresse);

            if (message == null)
                Assert.Fail("Das E-Mail mit Betreff \"{0}\" im Postfach für \"{1}\" konnte nicht gefunden werden.",
                    betreff, adresse);
            return message;
        }

    }
}