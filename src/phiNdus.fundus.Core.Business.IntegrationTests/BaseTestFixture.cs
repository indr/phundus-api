using System;
using Castle.Windsor;
using NUnit.Framework;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.IntegrationTests
{
    public class BaseTestFixture<T>
    {
        protected T Sut { get; set; }

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

        [SetUp]
        public virtual void SetUp()
        {
            
        }

        [TearDown]
        public virtual void TearDown()
        {
            
        }

        protected static string GetNewSessionKey()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}