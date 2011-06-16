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
            var request = WebRequest.Create("http://fundus.phindus.ch/staging");
            request.Timeout = 1000;

            var response = request.GetResponse();
            Assert.That(response, Is.Not.Null);

            var stream = new StreamReader(response.GetResponseStream());
            var content = stream.ReadToEnd();

            Assert.That(content, Contains.Substring("<h2>Home &gt; Index</h2>"));
        }
    }
}
