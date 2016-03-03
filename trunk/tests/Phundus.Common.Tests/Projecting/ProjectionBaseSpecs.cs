namespace Phundus.Common.Tests.Projecting
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Common.Domain.Model;
    using Common.Projecting;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Fakes;
    using Machine.Specifications;
    using NHibernate;
    using Rhino.Mocks;

    public class TestEntity
    {
        public int Id { get; set; }
        public Guid Value { get; set; }
    }

    public class TestProjection : ProjectionBase<TestEntity>
    {
        public override void Handle(DomainEvent e)
        {
            throw new NotImplementedException();
        }

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

    public class projection_base_concern : Observes<TestProjection>
    {
        protected static ISession session;

        private static int nextId = 1;
        protected static TestEntity entity;


        private static List<TestEntity> entities;

        protected static IQueryOver<TestEntity, TestEntity> query;

        private Establish ctx = () =>
        {
            entities = new List<TestEntity>();

            query = fake.an<IQueryOver<TestEntity, TestEntity>>();
            session = depends.on<ISession>();
            session.setup(x => x.QueryOver<TestEntity>()).Return(query);


            sut_factory.create_using(() =>
            {
                var sut = new TestProjection();
                sut.SessionFactory = () => session;
                return sut;
            });

            entity = makeEntity();
        };

        protected static TestEntity makeEntity()
        {
            var result = new TestEntity();
            result.Id = nextId++;
            entities.Add(result);
            return result;
        }
    }

    public class when_inserting_entity : projection_base_concern
    {
        private Because of = () =>
            sut.InsertEntity(entity);

        private It should_save_entity = () =>
            session.received(x => x.Save(Arg<TestEntity>.Is.Equal(entity)));
    }

    public class when_insert_with_action : projection_base_concern
    {
        private static Guid value = Guid.NewGuid();

        private Because of = () =>
            sut.InsertWithAction(value);

        private It should_save_new_entity = () =>
            session.received(x => x.Save(Arg<TestEntity>.Matches(p =>
                p.Value == value)));
    }

    public class when_updating_a_single_entity : projection_base_concern
    {
        private static Guid value = Guid.NewGuid();

        private Establish ctx = () =>
            session.setup(x => x.Get<TestEntity>(entity.Id)).Return(entity);

        private Because of = () =>
            sut.UpdateSingleEntity(entity.Id, value);

        private It should_execute_action_method = () =>
            entity.Value.ShouldEqual(value);

        private It should_update_entity = () =>
            session.received(x => x.Update(entity));
    }

    public class when_updating_a_single_entity_that_is_not_existing : projection_base_concern
    {
        private Because of = () =>
            spec.catch_exception(() =>
                sut.UpdateSingleEntity(-1, Guid.NewGuid()));

        private It should_throw_not_found_exception = () =>
            spec.exception_thrown.ShouldBeAn<InvalidOperationException>();

        private It should_throw_with_exception_message = () =>
            spec.exception_thrown.Message.ShouldEqual(
                "Could not update projection TestProjection. Projection TestEntity -1 not found.");
    }

    public class when_updating_multiple_entities_with_expression : projection_base_concern
    {
        private static Guid value = Guid.NewGuid();
        private static TestEntity entity2;
        private static TestEntity entity3;
        private static Expression<Func<TestEntity, bool>> expression;
        private static bool flushCalled;
        private static bool flushCalledBeforeQueryOver;

        private Establish ctx = () =>
        {
            entity2 = makeEntity();
            entity3 = makeEntity();

            session.setup(x => x.Flush()).Callback(() => flushCalled = true);

            var parameter = Expression.Parameter(typeof (TestEntity));
            var lambda = Expression.LessThan(Expression.Property(parameter, "Id"), Expression.Constant(entity3.Id));
            expression = Expression.Lambda<Func<TestEntity, bool>>(lambda, parameter);

            query.setup(x => x.Where(expression)).Return(query);
            query.setup(x => x.Future()).Return(() =>
            {
                flushCalledBeforeQueryOver = flushCalled;
                return new[] {entity, entity2};
            });
        };

        private Because of = () =>
            sut.UpdateEntities(expression, value);

        private It should_call_action_method = () =>
        {
            entity.Value.ShouldEqual(value);
            entity2.Value.ShouldEqual(value);
        };

        private It should_flush_before_query = () =>
            flushCalledBeforeQueryOver.ShouldBeTrue();

        private It should_not_call_action_method_on_not_affected_entity = () =>
            entity3.Value.ShouldNotEqual(value);

        private It should_update = () =>
        {
            session.received(x => x.Update(entity));
            session.received(x => x.Update(entity2));
        };
    }

    public class when_inserting_or_updating_with_expression : projection_base_concern
    {
        protected static Guid value;
        protected static Expression<Func<TestEntity, bool>> expression;
        protected static bool flushCalled;
        protected static bool flushcCalledBeforeSingleOrDefault;

        private Establish ctx = () =>
        {
            value = Guid.NewGuid();
            session.setup(x => x.Flush()).Callback(() => flushCalled = true);
        };

        private Because of = () =>
            sut.InsertOrUpdate(expression, value);
    }

    public class when_inserting_or_updating_with_expression_and_expression_returns_nothing :
        when_inserting_or_updating_with_expression
    {
        private Establish ctx = () =>
        {
            var parameter = Expression.Parameter(typeof (TestEntity));
            var lambda = Expression.Constant(false);
            expression = Expression.Lambda<Func<TestEntity, bool>>(lambda, parameter);

            query.setup(x => x.Where(expression)).Return(query);
            query.setup(x => x.SingleOrDefault()).Return(() =>
            {
                flushcCalledBeforeSingleOrDefault = flushCalled;
                return null;
            });            
        };

        private It should_flush_before_single_or_default = () =>
            flushcCalledBeforeSingleOrDefault.ShouldBeTrue();

        private It should_save_or_update_new_entity_with_value = () =>
            session.received(x => x.SaveOrUpdate(Arg<TestEntity>.Matches(p =>
                p.Id == 0 && p.Value == value)));
    }

    public class when_inserting_or_updating_with_expression_and_expression_returns_entity :
        when_inserting_or_updating_with_expression
    {
        private Establish ctx = () =>
        {
            var parameter = Expression.Parameter(typeof (TestEntity));
            var lambda = Expression.Constant(true);
            expression = Expression.Lambda<Func<TestEntity, bool>>(lambda, parameter);

            query.setup(x => x.Where(expression)).Return(query);
            query.setup(x => x.SingleOrDefault()).Return(() =>
            {
                flushcCalledBeforeSingleOrDefault = flushCalled;
                return entity;
            });
        };

        private It should_call_action_method = () =>
            entity.Value.ShouldEqual(value);

        private It should_flush_before_single_or_default = () =>
            flushcCalledBeforeSingleOrDefault.ShouldBeTrue();

        private It should_save_or_update_entity = () =>
            session.received(x => x.SaveOrUpdate(entity));
    }

    public class when_inserting_or_updating_entity : projection_base_concern
    {
        private Because of = () =>
            sut.InsertOrUpdateEntity(entity);

        private It should_save_or_update_entity = () =>
            session.received(x => x.SaveOrUpdate(entity));
    }

    public class when_deleting_with_object_as_id : projection_base_concern
    {
        private Establish ctx = () =>
            session.setup(x => x.Get<TestEntity>(1)).Return(entity);

        private Because of = () =>
            sut.DeleteById(1);

        private It should_delete_entity = () =>
            session.received(x => x.Delete(entity));
    }

    public class when_deleting_an_entity : projection_base_concern
    {
        private Because of = () =>
            sut.DeleteEntity(entity);

        private It should_delete_entity = () =>
            session.received(x => x.Delete(entity));
    }

    public class when_resetting : projection_base_concern
    {
        private Because of = () =>
            sut.Reset();

        private It should_delete_from = () =>
            session.received(x => x.Delete("FROM TestEntity"));
    }
}