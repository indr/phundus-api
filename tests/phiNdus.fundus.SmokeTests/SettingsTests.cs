using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Business;
using phiNdus.fundus.Core.Domain.Settings;
using phiNdus.fundus.TestHelpers;
using Rhino.Commons;

namespace phiNdus.fundus.SmokeTests
{
    [TestFixture]
    public class SettingsTests
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            IoC.Initialize(new WindsorContainer());
            IoC.Container.Install(new Installer());
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            IoC.Container.Dispose();
        }

        [Test]
        public void ServerUrlHasBeenSet()
        {
            var actual = Settings.Common.ServerUrl;

            var appSettings = new AppSettingsReader();
            var uri = appSettings.GetValue("uri", typeof(string)).ToString();

            Assert.That("http://" + actual, Is.EqualTo(uri));
        }
    }
}
