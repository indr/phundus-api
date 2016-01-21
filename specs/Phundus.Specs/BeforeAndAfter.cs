namespace Phundus.Specs
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using TechTalk.SpecFlow;

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