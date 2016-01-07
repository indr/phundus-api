namespace Phundus.Specs.Steps
{
    using System;
    using System.Configuration;
    using System.Reflection;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using WatiN.Core;

    [Binding]
    public class StartseiteSteps : StepBase
    {
        [When(@"ich den Shop aufrufe")]
        public void WennIchDenShopAufrufe()
        {
            Browser.GoTo(BaseUrl + "/shop");
        }

        [When(@"ich die Webseite aufrufe")]
        public void WennIchDieWebseiteAufrufe()
        {
            Browser.GoTo(BaseUrl);
        }

        [Then(@"sollte ich ein Heading 1 mit ""(.*)"" sehen")]
        public void DannSollteIchEinHeading1MitSehen(string p0)
        {
            Assert.That(Browser.ElementWithTag("h1", Find.ByIndex(0)).Text, Is.EqualTo(p0));
        }

        [Then(@"sollte ich gross ""(.*)"" sehen")]
        public void DannSollteIchGrossSehen(string p0)
        {
            Assert.That(Browser.ElementWithTag("h2", Find.ByIndex(0)).Text, Is.EqualTo(p0));
        }
        
        [Then(@"sollte im Fenstertitel muss ""(.*)"" stehen")]
        public void DannSollteImFenstertitelMussStehen(string p0)
        {
            Assert.That(Browser.Title, Is.EqualTo(p0));
        }

        [Then(@"sollte die Version entsprechend der zuletzt installierten Version sein")]
        public void DannSollteDieVersionEntsprechendDerZuletztInstalliertenVersionSein()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version.ToString(3);
            var revision = assembly.GetName().Version.Revision;
            var expected = String.Format("{0} (rev {1})", version, revision);

            Assert.That(Browser.Span(Find.ByClass("versionTag")).Text, Is.EqualTo(expected));
        }

        [Then(@"sollte die Server-URL entsprechend der Konfiguration gesetzt sein")]
        public void DannSollteDieServer_URLEntsprechendDerKonfigurationGesetztSein()
        {
            var expected = "http://" + ConfigurationManager.AppSettings["ServerUrl"];
            var actual = Browser.Span(Find.ByClass("serverUrlTag")).Link(Find.ByText("phundus")).Url;
            actual = actual.TrimEnd('/');
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Then(@"sollte ich als ""(.*)"" angemeldet sein")]
        public void DannSollteIchAlsAngemeldetSein(string userName)
        {            
            var span = Browser.Span(Find.ByClass("loggedOnTag", false));
            Assert.That(span, Is.Not.Null);
            Assert.That(span.InnerHtml, Is.EqualTo(userName));
        }

        [Then(@"ich sollte auf der Startseite sein")]
        public void DannIchSollteAufDerStartseiteSein()
        {
            Assert.That(Browser.Url, Is.EqualTo(BaseUrl + "/"));
        }

    }
}
