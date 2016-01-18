namespace Phundus.Specs.Www
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Runtime.Remoting;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class RobotsTxtSteps : Steps
    {
        private HttpWebResponse _response;

        protected static string BaseUrl
        {
            get
            {
                var result = ConfigurationManager.AppSettings["ServerUrl"];
                if (result.StartsWith("http"))
                    return result;
                return "http://" + result;
            }
        }

        private static HttpWebResponse GetWebResponse(string url)
        {
            return (HttpWebResponse)WebRequest.Create(BaseUrl + url).GetResponse();
        }

        [When(@"I try to request (.*)")]
        public void WhenRobots_TxtAufgerufenWird(string url)
        {
            _response = GetWebResponse(url);
        }

        [Then(@"I should get status (.*)")]
        public void ThenIShouldGetResponseCode(string statusDescription)
        {
            Assert.That(_response.StatusDescription, Is.EqualTo(statusDescription));
        }

        [Then(@"I should get content containing ""(.*)""")]
        public void ThenIShouldGetContentContaining(string text)
        {
            var stream = _response.GetResponseStream();

            Assert.That(stream, Is.Not.Null, "Could not get response stream.");
            var content = new StreamReader(stream).ReadToEnd();
            Assert.That(content, Contains.Substring(text));
        }
    }
}