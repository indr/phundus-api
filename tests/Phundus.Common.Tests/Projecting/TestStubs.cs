namespace Phundus.Common.Tests.Projecting
{
    using System;
    using System.Linq.Expressions;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Projecting;
    using Querying;

    public class TestGuidIdentity : GuidIdentity
    {
    }

    public class TestEntity
    {
        private static int _nextId = 1;

        public int Id { get; set; }
        public Guid Value { get; set; }

        public static TestEntity Create()
        {
            var result = new TestEntity();
            result.Id = _nextId++;
            return result;
        }
    }

    public class TestProjection : ProjectionBase<TestEntity>        
    {
        public void InsertEntity(TestEntity entity)
        {
            Insert(entity);
        }

        public void InsertWithAction(Guid value)
        {
            Insert(a => a.Value = value);
        }

        public void UpdateSingleEntity(int id, Guid value)
        {
            Update(id, a => a.Value = value);
        }

        public void UpdateEntities(Expression<Func<TestEntity, bool>> expression, Guid value)
        {
            Update(expression, a =>
                a.Value = value);
        }

        public void InsertOrUpdate(Expression<Func<TestEntity, bool>> expression, Guid value)
        {
            InsertOrUpdate(expression, a =>
                a.Value = value);
        }

        public void InsertOrUpdateEntity(TestEntity entity)
        {
            InsertOrUpdate(entity);
        }

        public void DeleteById(int id)
        {
            Delete(id);
        }

        public void DeleteEntity(TestEntity entity)
        {
            Delete(entity);
        }
    }

    public class TestQuery : QueryBase<TestEntity>
    {
        public new TestEntity Find(object id)
        {
            return base.Find(id);
        }

        public new TestEntity Find(GuidIdentity identity)
        {
            return base.Find(identity);
        }
    }
}