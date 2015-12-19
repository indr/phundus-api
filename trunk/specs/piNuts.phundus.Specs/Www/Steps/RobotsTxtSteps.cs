namespace Phundus.Specs.Www.Steps
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using NUnit.Framework;
    using piNuts.phundus.Specs.Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class RobotsTxtSteps : StepBase
    {        
        private HttpWebResponse _response;

        [When(@"/robots\.txt aufgerufen wird")]
        public void WennRobots_TxtAufgerufenWird()
        {
            _response = (HttpWebResponse) WebRequest.Create(BaseUrl + "/robots.txt").GetResponse();
        }

        [Then(@"muss der Http-Status (.*) sein")]
        public void DannMussDerHttp_StatusSein(int p0)
        {
            Assert.That(Convert.ToInt32(_response.StatusCode), Is.EqualTo(p0));
            _response.Close();
        }
    }
}