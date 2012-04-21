using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Settings;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Settings
{
    [TestFixture]
    public class CommonSettingsTests : SettingsTestFixture<ICommonSettings>
    {
        [Test]
        public void GetServerUrlDefaultsToLocalhost()
        {
            var actual = Sut.ServerUrl;
            Assert.That(actual, Is.EqualTo("localhost"));
        }

        [Test]
        public void GetServerUrl()
        {
            InsertSetting("common.server-url", "fundus.domain.ch");
            
            var actual = Sut.ServerUrl;
            Assert.That(actual, Is.EqualTo("fundus.domain.ch"));
        }

        protected override ICommonSettings CreateSut()
        {
            return new SettingsImpl().Common;
        }
    }
}
