namespace Phundus.Specs.Services
{
    using System;
    using System.Configuration;
    using BoDi;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using TechTalk.SpecFlow;

    [Binding]
    public class WindsorContainerSupport
    {
        private readonly IObjectContainer _objectContainer;

        public WindsorContainerSupport(IObjectContainer objectContainer)
        {
            if (objectContainer == null) throw new ArgumentNullException("objectContainer");
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void InitializeWindsorContainer()
        {
            var container = new WindsorContainer();

            _objectContainer.RegisterInstanceAs<IWindsorContainer>(container);

            if (ConfigurationManager.AppSettings["ServerUrl"] == "localhost")
                _objectContainer.RegisterTypeAs<RestMailbox, IMailbox>();
            else
                _objectContainer.RegisterTypeAs<PopMailbox, IMailbox>();
        }
    }
}