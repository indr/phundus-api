using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace phiNdus.fundus.SmokeTests
{
    [TestFixture]
    public class DeploymentTests
    {
        [Test]
        public void Can_get_home_page()
        {
            var appSettings = new System.Configuration.AppSettingsReader();
            var uri = appSettings.GetValue("uri", typeof (string)).ToString();

            var request = WebRequest.Create(uri);
            request.Timeout = 5000;

            var response = request.GetResponse();
            Assert.That(response, Is.Not.Null);

            var stream = new StreamReader(response.GetResponseStream());
            var content = stream.ReadToEnd();

            Assert.That(content, Contains.Substring("<h2>Home</h2>"));
        }
    }
}
