namespace Phundus.Tests
{
    using System;
    using Common.Projecting;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using NHibernate;
    using Rhino.Mocks;

    public class projection_concern<TProjection, TData> : Observes<TProjection>
        where TProjection : ProjectionBase<TData> where TData : class, new()
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
            session.setup(x => x.Save(Arg<TData>.Is.Anything)).Return((Func<TData, object>) (arg =>
            {
                entity = arg;
                return arg;
            }));
        };

        protected static void inserted(Action<TData> action)
        {
            session.received(x => x.Save(Arg<TData>.Is.NotNull));
            action(entity);
        }

        protected static void updated(object id, Action<TData> action)
        {
            session.received(x => x.Get<TData>(id));
            session.received(x => x.Update(entity));
            action(entity);
        }
    }
}