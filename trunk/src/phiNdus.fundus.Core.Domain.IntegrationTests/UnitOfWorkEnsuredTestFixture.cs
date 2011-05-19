using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Bootstrapper;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests
{
    internal class UnitOfWorkEnsuredTestFixture
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            IoC.Initialize(new WindsorContainer());
            IoC.Container.Register(Component.For<IUnitOfWorkFactory>().Instance(
                new NHibernateUnitOfWorkFactory(new[] { Assembly.GetAssembly(typeof(BaseEntity)) })));
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            IoC.Container.Dispose();
        }
    }
}