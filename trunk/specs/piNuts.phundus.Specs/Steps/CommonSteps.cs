using System;
using System.Configuration;
using System.Reflection;
using NUnit.Framework;
using OpenPop.Mime;
using piNuts.phundus.Specs.Infrastructure;
using TechTalk.SpecFlow;
using WatiN.Core;

namespace piNuts.phundus.Specs.Steps
{
    [Binding]
    public class CommonSteps : StepBase
    {
        [Given(@"ich bin nicht angemeldet")]
        public void AngenommenIchBinNichtAngemeldet()
        {
            if (Browser.Url == "about:blank")
                return;

            var link = Browser.Link(Find.ByClass("logOffTag"));

            if (!link.Exists)
                return;
            Browser.GoTo(link.Url);
        }

        [Given(@"ich tippe ins Feld ""(.*)"" ""(.*)"" ein")]
        public void AngenommenIchTippeInsFeldEin(string feld, string text)
        {
            text = text.Replace("{AppSettings.ServerUrl}", ConfigurationManager.AppSettings["ServerUrl"]);
            text = text.Replace("{Assembly.Version}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Browser.TextField(Find.ByLabelText(feld)).TypeText(text);
        }

        [Given(@"ich tippe ins Feld ""(.*)"" ein:")]
        public void AngenommenIchTippeInsFeld(string feld, string multilineText)
        {
            AngenommenIchTippeInsFeldEin(feld, multilineText);
        }

        [Given(@"ich füge ins Feld ""(.*)"" ""(.*)"" ein")]
        public void AngenommenIchFügeInsFeldEin(string feld, string text)
        {
            text = text.Replace("{AppSettings.ServerUrl}", ConfigurationManager.AppSettings["ServerUrl"]);
            text = text.Replace("{Assembly.Version}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Browser.TextField(Find.ByLabelText(feld)).Value = text;
        }

        [Given(@"ich füge ins Feld ""(.*)"" ein:")]
        public void AngenommenIchFügeInsFeld(string feld, string multilineText)
        {
            AngenommenIchFügeInsFeldEin(feld, multilineText);
        }

        [When(@"ich auf ""(.*)"" klicke")]
        public void WennIchAufDrucke(string value)
        {
            Browser.Button(Find.ByValue(value)).Click();
        }

        [Then(@"muss die Meldung ""(.*)"" erscheinen")]
        public void DannMussDieMeldungErscheinen(string meldung)
        {
            Assert.That(Browser.ContainsText(meldung), Is.True,
                String.Format("Die Meldung \"{0}\" ist nicht vorhanden.", meldung));
        }

        [Then(@"muss das Feld ""(.*)"" rot sein")]
        public void DannMussDasFeldRotSein(string p0)
        {
            Assert.That(Browser.Label(p => p.Text == p0).Parent.ClassName, Is.EqualTo("control-group error"));
        }

        [Then(@"muss ""(.*)"" ein E-Mail erhalten mit dem Betreff ""(.*)""")]
        public Message DannMussEinEMailErhaltenMitDemBetreff(string adresse, string betreff)
        {
            var mailbox = Mailbox.For(adresse);
            var message = mailbox.Find(betreff);

            if (message == null)
                Assert.Fail(
                    String.Format(
                        "Das E-Mail mit Betreff \"{0}\" im Postfach für \"{1}\" konnte nicht gefunden werden.",
                        betreff, adresse));
            return message;
        }

        [Then(@"muss ""(.*)"" ein E-Mail erhalten mit dem Betreff ""(.*)"" und dem Text:")]
        public void DannMussEinEMailErhaltenMitDemBetreffUndDemText(string adresse, string betreff, string text)
        {
            var message = DannMussEinEMailErhaltenMitDemBetreff(adresse, betreff);
            Assert.That(message.FindFirstPlainTextVersion().GetBodyAsText(), Is.EqualTo(text));
        }
    }
}