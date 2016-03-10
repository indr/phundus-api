namespace Phundus.Persistence.Tests.Notifications
{
    using System;
    using System.Linq.Expressions;
    using Common.Notifications;
    using developwithpassion.specifications.extensions;
    using Machine.Specifications;
    using NHibernate;
    using Persistence.Notifications;
    using Rhino.Mocks;

    public class nh_tracker_repository_concern : concern<NhTrackerRepository>
    {
        protected static bool flushCalled;
        protected static bool flushCalledBeforeQueryOver;
        protected static IQueryOver<ProcessedNotificationTracker, ProcessedNotificationTracker> query;

        private Establish ctx = () =>
        {
            flushCalled = false;
            flushCalledBeforeQueryOver = false;

            query = fake.an<IQueryOver<ProcessedNotificationTracker, ProcessedNotificationTracker>>();
            query.setup(x => x.Where(Arg<Expression<Func<ProcessedNotificationTracker, bool>>>.Is.Anything))
                .Return(query);

            session.setup(x => x.Flush()).Callback(() => flushCalled = true);
            session.setup(x => x.QueryOver<ProcessedNotificationTracker>()).Return(() =>
            {
                flushCalledBeforeQueryOver = flushCalled;
                return query;
            });
        };
    }

    public class when_find : nh_tracker_repository_concern
    {
        private static ProcessedNotificationTracker tracker;
        private static ProcessedNotificationTracker returnValue;

        private Establish ctx = () =>
        {
            tracker = fake.an<ProcessedNotificationTracker>();
            query.setup(x => x.SingleOrDefault()).Return(tracker);
        };

        private Because of = () =>
            returnValue = sut.Find("trackerName");

        private It should_flush_session_before_query_over = () =>
            flushCalledBeforeQueryOver.ShouldBeTrue();

        private It should_return_tracker = () =>
            returnValue.ShouldBeTheSameAs(tracker);
    }
}