namespace Phundus.Tests
{
    using System;
    using Common.Projecting;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Fakes;
    using Machine.Specifications;
    using NHibernate;
    using Rhino.Mocks;

    public class projection_concern<TSut, TData> : Observes<TSut> where TSut : ProjectionBase where TData : new()
    {
        protected static ISession session;
        protected static TData entity;

        private Establish ctx = () =>
        {
            session = fake.an<ISession>();

            sut_setup.run(sut =>
                sut.SessionFactory = () => session);

            entity = new TData();

            session.setup(x => x.Get<TData>(Arg<object>.Is.Anything)).Return(entity);
        };

        protected static void updated(object id, Action<TData> action)
        {
            session.received(x => x.Get<TData>(id));
            session.received(x => x.Update(entity));
            action(entity);
        }
    }
}