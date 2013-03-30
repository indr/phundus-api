using System;
using System.Configuration;
using System.IO;
using System.Net;
using NUnit.Framework;
using piNuts.phundus.Specs.Browsers;
using TechTalk.SpecFlow;

namespace piNuts.phundus.Specs
{
    /// <summary>
    /// http://volaresystems.com/Blog/post/2013/01/06/SpecFlow-and-WatiN-Worst-Practices-What-NOT-to-do.aspx
    /// </summary>
    [Binding]
    public class BeforeAndAfter
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            WarmUp();
        }

        private static void WarmUp()
        {
            var appSettings = new AppSettingsReader();
            var url = "http://" + appSettings.GetValue("ServerUrl", typeof (string)).ToString();

            var request = WebRequest.Create(url);
            request.Timeout = Convert.ToInt32(TimeSpan.FromMinutes(2).TotalMilliseconds);

            var response = request.GetResponse();
            Assert.That(response, Is.Not.Null);

            var stream = new StreamReader(response.GetResponseStream());
            stream.ReadToEnd();
            //var content = stream.ReadToEnd();
            //Assert.That(content, Contains.Substring(@"<div class=""hero-unit"">"));
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Browser.Current.Close();
        }
    }
}