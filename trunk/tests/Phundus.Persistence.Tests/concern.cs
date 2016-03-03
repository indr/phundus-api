namespace Phundus.Persistence.Tests
{
    using System;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Fakes;
    using Machine.Specifications;
    using NHibernate;

    public abstract class concern<TClass> : Observes<TClass> where TClass : class
    {
        private Establish ctx = () =>
        {
            session = depends.on<ISession>();
            depends.on<Func<ISession>>(() => session);

            
        };

        protected static ISession session;
    }
}