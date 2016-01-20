namespace Phundus.Specs
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using ContentTypes;
    using NUnit.Framework;
    using Phundus.Specs.Browsers;
    using Services;
    using TechTalk.SpecFlow;

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
            DeleteEmails();
        }

        private static void WarmUp()
        {
            var appSettings = new AppSettingsReader();
            var url = appSettings.GetValue("ServerUrl", typeof (string)).ToString();
            if (!url.StartsWith("http://"))
                url = "http://" + url;
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
            if (ConfigurationManager.AppSettings["ForceClose"] == "true")
                Browser.Current.ForceClose();
            else
                Browser.Current.Close();
        }

        private static void DeleteEmails()
        {
            new Resource("sessions", false).Post(new SessionsPostRequestContent
            {
                Username = "admin@test.phundus.ch",
                Password = "1234"
            });
            new Resource("mails", false).Delete();
        }
    }
}