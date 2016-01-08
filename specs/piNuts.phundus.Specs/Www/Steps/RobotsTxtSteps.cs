namespace Phundus.Specs.Www.Steps
{
    using System;
    using System.Net;
    using NUnit.Framework;
    using Specs.Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class RobotsTxtSteps : StepBase
    {
        private HttpWebResponse _response;

        [When(@"/robots\.txt aufgerufen wird")]
        public void WhenRobots_TxtAufgerufenWird()
        {
            _response = (HttpWebResponse) WebRequest.Create(BaseUrl + "/robots.txt").GetResponse();
        }

        [Then(@"muss der Http-Status (.*) sein")]
        public void ThenMussDerHttp_StatusSein(int p0)
        {
            Assert.That(Convert.ToInt32(_response.StatusCode), Is.EqualTo(p0));
            _response.Close();
        }
    }
}