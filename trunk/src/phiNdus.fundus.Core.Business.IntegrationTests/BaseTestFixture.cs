using System;
using Castle.Windsor;
using NUnit.Framework;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.IntegrationTests
{
    public class BaseTestFixture
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

        protected static string GetNewSessionKey()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}