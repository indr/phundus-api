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
        public void WhenIchDenShopAufrufe()
        {
            Browser.GoTo(BaseUrl + "/shop");
        }
       
        [Then(@"sollte im Fenstertitel muss ""(.*)"" stehen")]
        public void ThenSollteImFenstertitelMussStehen(string p0)
        {
            Assert.That(Browser.Title, Is.EqualTo(p0));
        }

        [Then(@"sollte die Version entsprechend der zuletzt installierten Version sein")]
        public void ThenSollteDieVersionEntsprechendDerZuletztInstalliertenVersionSein()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version.ToString(3);
            var revision = assembly.GetName().Version.Revision;
            var expected = String.Format("{0} (rev {1})", version, revision);

            Assert.That(Browser.Span(Find.ByClass("versionTag")).Text, Is.EqualTo(expected));
        }
    }
}
