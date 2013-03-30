using System;
using System.Configuration;
using System.Reflection;
using NUnit.Framework;
using piNuts.phundus.Specs.Infrastructure;
using TechTalk.SpecFlow;
using WatiN.Core;

namespace piNuts.phundus.Specs.Steps
{
    [Binding]
    public class FeedbackSteps : StepBase
    {
        [Given(@"ich bin auf der Feedbackseite")]
        public void AngenommenIchBinAufDerFeedbackseite()
        {
            Browser.GoTo(BaseUrl + "/feedback");
        }

        [Given(@"ich tippe ins Feld ""(.*)"" ""(.*)"" ein")]
        public void AngenommenIchTippeInsFeldEin(string p0, string p1)
        {
            p1 = p1.Replace("{AppSettings.ServerUrl}", ConfigurationManager.AppSettings["ServerUrl"]);
            p1 = p1.Replace("{Assembly.Version}", Assembly.GetExecutingAssembly().GetName().Version.ToString());

            Browser.TextField(Find.ByLabelText(p0)).TypeText(p1);
        }

        [Given(@"ich tippe ins Feld ""(.*)"":")]
        public void AngenommenIchTippeInsFeld(string p0, string multilineText)
        {
            AngenommenIchTippeInsFeldEin(p0, multilineText);
        }

        [When(@"ich auf ""(.*)"" drücke")]
        public void WennIchAufDrucke(string p0)
        {
            Browser.Button(Find.ByValue(p0)).Click();
        }

        [Then(@"muss die Meldung ""(.*)"" erscheinen")]
        public void DannMussNebenDemFeldErscheinen(string p0)
        {
            Assert.That(Browser.ContainsText(p0), Is.True);
        }

        [Then(@"muss das Feld ""(.*)"" rot sein")]
        public void DannMussDasFeldRotSein(string p0)
        {
            Assert.That(Browser.Label(p => p.Text == p0).Parent.ClassName, Is.EqualTo("control-group error"));
        }

        [Then(@"muss ""(.*)"" ein E-Mail mit dem Betreff ""(.*)"" erhalten haben")]
        public void DannMussEinFeedback_E_MailErhaltenHaben(string p0, string p1)
        {
            var mailbox = Mailbox.For(p0);
            var message = mailbox.Find(p1);

            if (message == null)
                Assert.Fail(String.Format("Das E-Mail mit Betreff \"{0}\" im Postfach für \"{1}\" konnte nicht gefunden werden.",
                    p1, p0));
        }

    }
}