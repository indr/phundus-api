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
        private string _body;
        private HttpWebResponse _response;

        [When(@"/robots\.txt aufgerufen wird")]
        public void WennRobots_TxtAufgerufenWird()
        {
            _response = (HttpWebResponse) WebRequest.Create(BaseUrl + "/robots.txt").GetResponse();
        }

        [When(@"/api/v1/status aufgerufen wird")]
        public void WennApiV1StatusAufgerufenWird()
        {
            //_response = (HttpWebResponse)WebRequest.Create(BaseUrl + "/Content/status.txt").GetResponse();
            _response = (HttpWebResponse) WebRequest.Create(NodeUrl + "/status").GetResponse();

            var stream = _response.GetResponseStream();
            if (null != stream) _body = new StreamReader(stream).ReadToEnd();
        }

        [Then(@"muss der Http-Status (.*) sein")]
        public void DannMussDerHttp_StatusSein(int p0)
        {
            Assert.That(Convert.ToInt32(_response.StatusCode), Is.EqualTo(p0));
            _response.Close();
        }

        [Then(@"JSON status: 'OK'")]
        public void DannJSONStatusOK()
        {
            var regex = new Regex(@"""status"":\s*""OK""\s*,");
            Assert.That(regex.Match(_body).Success, Is.True, _body);
        }
    }
}