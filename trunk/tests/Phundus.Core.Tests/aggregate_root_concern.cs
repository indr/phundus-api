namespace Phundus.Tests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Common.Domain.Model;
    using Machine.Specifications;

    public class aggregate_root_concern<TAggregate> : aggregate_concern<TAggregate> where TAggregate : EventSourcedAggregateRoot
    {
        protected static void mutatingEvent<T>(Expression<Func<T, bool>> predicate)
        {
            var e = sut.MutatingEvents.SingleOrDefault(p => p.GetType() == typeof(T));

            var typed = (T)e;

            typed.ShouldMatch(predicate);
        }

        protected static void mutatingEvent<T>(Action<T> a)
        {
            var e = sut.MutatingEvents.SingleOrDefault(p => p.GetType() == typeof (T));
            a((T)e);
        }

        protected static T mutatingEvent<T>()
        {
            return (T)sut.MutatingEvents.SingleOrDefault(p => p.GetType() == typeof (T));
        }
    }
}