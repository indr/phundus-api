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

        [Given(@"ich bin auf der Seite ""(.*)""")]
        public void GivenIchBinAufDerSeite(string url)
        {
            Browser.GoTo(BaseUrl + url);
        }

        [Given(@"Ich bin auf der Startseite")]
        public void GivenIchBinAufDerStartseite()
        {
            Browser.GoTo(BaseUrl + "/shop");
        }

        [Given(@"ich bin auf der Registrierenseite")]
        public void GivenIchBinAufDerRegistrierenseite()
        {
            Browser.GoTo(BaseUrl + "/account/signup");
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

        [Given(@"ich bin als Verwalter angemeldet")]
        public void GivenIchBinAlsVerwalterAngemeldet()
        {
            Login("chief@test.phundus.ch", "1234");
        }

        [Given(@"ich bin als Administrator angemeldet")]
        public void GivenIchBinAlsAdministratorAngemeldet()
        {
            Login("admin@test.phundus.ch", "1234");
        }

        [Given(@"ich tippe ins Feld ""(.*)"" ""(.*)"" ein")]
        public void GivenIchTippeInsFeldEin(string feld, string text)
        {
            text = text.Replace("{AppSettings.ServerUrl}", ConfigurationManager.AppSettings["ServerUrl"]);
            text = text.Replace("{Assembly.Version}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            var textField = Browser.TextField(Find.ByLabelText(feld));
            textField.TypeText(text);
        }

        [Given(@"ich tippe ins Feld ""(.*)"" ein:")]
        public void GivenIchTippeInsFeld(string feld, string multilineText)
        {
            GivenIchTippeInsFeldEin(feld, multilineText);
        }

        [Given(@"ich füge ins Feld ""(.*)"" ""(.*)"" ein")]
        public void GivenIchFügeInsFeldEin(string feld, string text)
        {
            text = text.Replace("{AppSettings.ServerUrl}", ConfigurationManager.AppSettings["ServerUrl"]);
            text = text.Replace("{Assembly.Version}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Browser.TextField(Find.ByLabelText(feld)).Value = text;
        }

        [Given(@"ich füge ins Feld ""(.*)"" ein:")]
        public void GivenIchFügeInsFeld(string feld, string multilineText)
        {
            GivenIchFügeInsFeldEin(feld, multilineText);
        }

        [When(@"ich auf ""(.*)"" klicke")]
        public void WhenIchAufDrucke(string value)
        {
            Browser.Button(Find.ByValue(value).Or(Find.ByText(value))).Click();
        }

        [Then(@"muss die Meldung ""(.*)"" erscheinen")]
        public void ThenMussDieMeldungErscheinen(string meldung)
        {
            Assert.That(Browser.ContainsText(meldung), Is.True,
                String.Format("Die Meldung \"{0}\" ist nicht vorhanden.", meldung));
        }

        [Then(@"muss das Feld ""(.*)"" rot sein")]
        public void ThenMussDasFeldRotSein(string p0)
        {
            Assert.That(Browser.Label(p => p.Text == p0).Parent.ClassName, Is.EqualTo("control-group error"));
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

        [Then(@"muss ""(.*)"" ein E-Mail erhalten mit dem Betreff ""(.*)"" und dem Text:")]
        public void ThenMussEinEMailErhaltenMitDemBetreffUndDemText(string adresse, string betreff, string text)
        {
            var message = DannMussEinEMailErhaltenMitDemBetreff(adresse, betreff);
            Assert.That(message.TextBody, Is.EqualTo(text));
        }
    }
}