using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Bootstrapper;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests
{
    internal class BaseTestFixture
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            IoC.Initialize(new WindsorContainer());
            IoC.Container.Install(new DomainLayerInstaller());
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            IoC.Container.Dispose();
        }
    }
}