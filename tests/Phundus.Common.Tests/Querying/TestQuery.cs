namespace Phundus.Common.Tests.Querying
{
    using System;
    using Common.Domain.Model;
    using Common.Querying;

    public class TestQuery : QueryServiceBase<TestQueryEntity>
    {
        public new TestQueryEntity Find(object id)
        {
            return base.Find(id);
        }

        public new TestQueryEntity Find(GuidIdentity identity)
        {
            return base.Find(identity);
        }
    }

    public class TestQueryEntityId : GuidIdentity
    {
    }

    public class TestQueryEntity
    {
        private static int _nextId = 1;

        public int Id { get; set; }
        public Guid Value { get; set; }

        public static TestQueryEntity Create()
        {
            var result = new TestQueryEntity();
            result.Id = _nextId++;
            return result;
        }
    }
}