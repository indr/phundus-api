using Castle.Windsor;
using NUnit.Framework;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests
{
    internal class BaseTestFixture
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
    }
}