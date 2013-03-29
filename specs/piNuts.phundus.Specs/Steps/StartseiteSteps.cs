using System;
using TechTalk.SpecFlow;

namespace piNuts.phundus.Specs.Steps
{
    using NUnit.Framework;

    [Binding]
    public class StartseiteSteps : StepBase
    {
        [When(@"ich die Webseite aufrufe")]
        public void WennIchDieWebseiteAufrufe()
        {
            Browser.GoTo(BaseUrl);
        }
        
        [Then(@"sollte ich gross ""(.*)"" sehen")]
        public void DannSollteIchGrossSehen(string p0)
        {
            Assert.That(Browser.ContainsText(p0), Is.True);
        }
        
        [Then(@"sollte im Fenstertitel muss ""(.*)"" stehen")]
        public void DannSollteImFenstertitelMussStehen(string p0)
        {
            Assert.That(Browser.Title, Is.EqualTo(p0));
        }
    }
}
