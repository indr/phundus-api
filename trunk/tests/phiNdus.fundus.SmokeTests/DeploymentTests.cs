﻿using System;
using System.Configuration;
using System.IO;
using System.Net;
using NUnit.Framework;

namespace phiNdus.fundus.SmokeTests
{
    [TestFixture]
    public class DeploymentTests
    {
        [Test]
        public void CanNavigateToHomePage()
        {
            var appSettings = new AppSettingsReader();
            var uri = appSettings.GetValue("uri", typeof (string)).ToString();

            var request = WebRequest.Create(uri);
            request.Timeout = Convert.ToInt32(TimeSpan.FromSeconds(30).TotalMilliseconds);

            var response = request.GetResponse();
            Assert.That(response, Is.Not.Null);

            var stream = new StreamReader(response.GetResponseStream());
            var content = stream.ReadToEnd();

            Assert.That(content, Contains.Substring("<h2>Home</h2>"));
        }
    }
}