namespace Phundus.Common.Tests.Projecting
{
    using System;
    using System.Linq.Expressions;
    using Common.Projecting;

    public class TestProjectionEntity
    {
        private static int _nextId = 1;

        public int Id { get; set; }
        public Guid Value { get; set; }

        public static TestProjectionEntity Create()
        {
            var result = new TestProjectionEntity();
            result.Id = _nextId++;
            return result;
        }
    }

    public class TestProjection : ProjectionBase<TestProjectionEntity>
    {
        public void InsertEntity(TestProjectionEntity entity)
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

        public void UpdateEntities(Expression<Func<TestProjectionEntity, bool>> expression, Guid value)
        {
            Update(expression, a =>
                a.Value = value);
        }

        public void InsertOrUpdate(Expression<Func<TestProjectionEntity, bool>> expression, Guid value)
        {
            InsertOrUpdate(expression, a =>
                a.Value = value);
        }

        public void InsertOrUpdateEntity(TestProjectionEntity entity)
        {
            InsertOrUpdate(entity);
        }

        public void DeleteById(int id)
        {
            Delete(id);
        }

        public void DeleteEntity(TestProjectionEntity entity)
        {
            Delete(entity);
        }
    }
}