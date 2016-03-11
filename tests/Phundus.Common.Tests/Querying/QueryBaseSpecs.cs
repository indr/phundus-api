namespace Phundus.Common.Tests.Querying
{
    using System.Collections.Generic;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using NHibernate;
    using Projecting;

    public class query_base_concern : Observes<TestQuery>
    {
        protected static ISession session;
        protected static IQueryOver<TestQueryEntity, TestQueryEntity> query;

        protected static TestQueryEntity entity;
        private static List<TestQueryEntity> entities;

        private Establish ctx = () =>
        {
            entities = new List<TestQueryEntity>();

            query = fake.an<IQueryOver<TestQueryEntity, TestQueryEntity>>();
            session = depends.on<ISession>();
            session.setup(x => x.QueryOver<TestQueryEntity>()).Return(query);

            sut_factory.create_using(() =>
            {
                var sut = new TestQuery();
                sut.SessionFactory = () => session;
                return sut;
            });

            entity = TestQueryEntity.Create();
            entities.Add(entity);
        };
    }

    public class when_find_with_integer : query_base_concern
    {
        private static int id = 123;
        private static TestQueryEntity returnValue;

        private Establish ctx = () =>
            session.setup(x => x.Get<TestQueryEntity>(id)).Return(entity);

        private Because of = () =>
            returnValue = sut.Find(id);

        private It should_return_entity = () =>
            returnValue.ShouldBeTheSameAs(entity);
    }

    public class when_find_with_guid_identity : query_base_concern
    {
        private static GuidIdentity identity;
        private static TestQueryEntity returnValue;

        private Establish ctx = () =>
        {
            identity = new TestQueryEntityId();
            session.setup(x => x.Get<TestQueryEntity>(identity.Id)).Return(entity);
        };

        private Because of = () =>
            returnValue = sut.Find(identity);

        private It should_return_entity = () =>
            returnValue.ShouldBeTheSameAs(entity);
    }

    public class when_find_with_guid_identity_and_entity_does_not_exist : query_base_concern
    {
        private static TestQueryEntityId identity;
        private static TestQueryEntity result;

        private Establish ctx = () =>
        {
            identity = new TestQueryEntityId();
            session.setup(x => x.Get<TestProjectionEntity>(identity)).Return((TestProjectionEntity) null);
        };

        private Because of = () =>
            spec.catch_exception(() =>
                result = sut.Find(identity));

        private It should_not_throw_exception = () =>
            spec.exception_thrown.ShouldBeNull();

        private It should_return_null = () =>
            result.ShouldBeNull();
    }
}