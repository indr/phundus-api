using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using NUnit.Framework;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests
{
    public class BaseTestFixture
    {
        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            Container = new WindsorContainer();
            IoC.Initialize(Container);

            // Todo,Inder: Mir ist schleierhaft, warum <mapping assembly="phiNdus.fundus.Core.Domain" /> im App.config nicht funktioniert.
            _factory = new NHibernateUnitOfWorkFactory(new Assembly[] { Assembly.UnsafeLoadFrom("phiNdus.fundus.Core.Domain.dll") });
            //_factory = new NHibernateUnitOfWorkFactory(new Assembly[] { Assembly.GetAssembly(typeof(User)) });

            Container.Install(
                new RepositoriesInstaller());
            Container.Register(Component.For<IUnitOfWorkFactory>().Instance(_factory));
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            Container.Dispose();
        }

        private IUnitOfWorkFactory _factory;

        protected IWindsorContainer Container;
    }
}