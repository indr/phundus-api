using System.Configuration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Business;
using Rhino.Commons;
using WatiN.Core;

namespace phiNdus.fundus.AcceptanceTests
{
    public class DslTestCase
    {
        protected string BaseUri { get; private set; }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            IoC.Initialize(new WindsorContainer());
            IoC.Container.Install(new Installer());

            var appSettings = new AppSettingsReader();
            BaseUri = appSettings.GetValue("uri", typeof (string)).ToString();

            Context = new TestContext { BaseUri = BaseUri, Browser = new IE() };
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            Context.Dispose();

            IoC.Container.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            
        }

        [TearDown]
        public void TearDown()
        {
           
        }

        protected TestContext Context { get; private set; }
    }
}