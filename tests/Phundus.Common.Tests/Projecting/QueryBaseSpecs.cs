namespace Phundus.Common.Tests.Projecting
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using NHibernate;

    public class query_base_concern : Observes<TestQuery>
    {
        protected static ISession session;
        protected static IQueryOver<TestEntity, TestEntity> query;

        protected static TestEntity entity;
        private static List<TestEntity> entities;

        private Establish ctx = () =>
        {
            entities = new List<TestEntity>();

            query = fake.an<IQueryOver<TestEntity, TestEntity>>();
            session = depends.on<ISession>();
            session.setup(x => x.QueryOver<TestEntity>()).Return(query);

            sut_factory.create_using(() =>
            {
                var sut = new TestQuery();
                sut.SessionFactory = () => session;
                return sut;
            });

            entity = TestEntity.Create();
            entities.Add(entity);
        };
    }

    public class when_find_with_integer : query_base_concern
    {
        private static int id = 123;
        private static TestEntity returnValue;

        private Establish ctx = () =>
            session.setup(x => x.Get<TestEntity>(id)).Return(entity);

        private Because of = () =>
            returnValue = sut.Find(id);

        private It should_return_entity = () =>
            returnValue.ShouldBeTheSameAs(entity);
    }

    public class when_find_with_guid_identity : query_base_concern
    {
        private static GuidIdentity identity;
        private static TestEntity returnValue;

        private Establish ctx = () =>
        {
            identity = new TestGuidIdentity();
            session.setup(x => x.Get<TestEntity>(identity.Id)).Return(entity);
        };

        private Because of = () =>
            returnValue = sut.Find(identity);

        private It should_return_entity = () =>
            returnValue.ShouldBeTheSameAs(entity);
    }

    public class when_find_with_guid_identity_and_entity_does_not_exist : query_base_concern
    {
        private static TestGuidIdentity identity;
        private static TestEntity returnValue;

        private Establish ctx = () =>
        {
            identity = new TestGuidIdentity();
            session.setup(x => x.Get<TestEntity>(identity)).Return((TestEntity) null);
        };

        private Because of = () =>
            spec.catch_exception(() =>
                returnValue = sut.Find(identity));

        private It should_not_throw_exception = () =>
            spec.exception_thrown.ShouldBeNull();

        private It should_return_null = () =>
            returnValue.ShouldBeNull();
    }
}