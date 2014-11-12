namespace Phundus.Core.Specs.Contexts
{
    using System;
    using InMemoryNHibernate;
    using NHibernate;
    using TechTalk.SpecFlow;

    [Binding]
    public class InMemorySessionSupport
    {
        private readonly Depends _depends;
        private readonly InMemorySession _inMemorySession;

        public InMemorySessionSupport(Depends depends, InMemorySession inMemorySession)
        {
            _depends = depends;
            _inMemorySession = inMemorySession;
        }

        [BeforeScenario]
        public void Initialize()
        {
            Func<ISession> func = () => _inMemorySession;
            _depends.On<Func<ISession>>(func);
            _depends.On<ISession>(_inMemorySession);
        }
    }
}