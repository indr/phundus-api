using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.TestHelpers;

namespace phiNdus.fundus.SmokeTests
{
    [TestFixture]
    public class DeploymentTests
    {
        [Test]
        public void Can_get_home_page()
        {
            // TODO: Smoke-Test sollte Datenbank berühren

            var appSettings = new System.Configuration.AppSettingsReader();
            var uri = appSettings.GetValue("uri", typeof (string)).ToString();

            var request = WebRequest.Create(uri);
            request.Timeout = Convert.ToInt32(TimeSpan.FromSeconds(20).TotalMilliseconds);

            var response = request.GetResponse();
            Assert.That(response, Is.Not.Null);

            var stream = new StreamReader(response.GetResponseStream());
            var content = stream.ReadToEnd();

            Assert.That(content, Contains.Substring("<h2>Home</h2>"));
        }
    }
}
