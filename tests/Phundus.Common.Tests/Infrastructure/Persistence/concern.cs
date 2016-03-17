namespace Phundus.Persistence.Tests
{
    using System;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using NHibernate;

    public abstract class concern<TClass> : Observes<TClass> where TClass : class
    {
        protected static ISession session;

        private Establish ctx = () =>
        {
            session = depends.on<ISession>();
            depends.on<Func<ISession>>(() => session);
        };
    }
}