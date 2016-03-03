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

    public class processed_notification_tracker_store_concern : concern<ProcessedNotificationTrackerStore>
    {
        protected static IQueryOver<ProcessedNotificationTracker, ProcessedNotificationTracker> query;

        private Establish ctx = () =>
        {
            query = fake.an<IQueryOver<ProcessedNotificationTracker, ProcessedNotificationTracker>>();
            query.setup(x => x.Where(Arg<Expression<Func<ProcessedNotificationTracker, bool>>>.Is.Anything))
                .Return(query);


            session.setup(x => x.QueryOver<ProcessedNotificationTracker>()).Return(query);
        };
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class get_processed_notification_tracker : processed_notification_tracker_store_concern
    {
        private static ProcessedNotificationTracker tracker;
        private static ProcessedNotificationTracker returnValue;

        private Establish ctx = () =>
        {
            tracker = fake.an<ProcessedNotificationTracker>();
            query.setup(x => x.SingleOrDefault()).Return(tracker);
        };

        private Because of = () =>
            returnValue = sut.GetProcessedNotificationTracker("typeName");

        private It should_return_tracker = () =>
            returnValue.ShouldBeTheSameAs(tracker);
    }

    [Subject(typeof(ProcessedNotificationTrackerStore))]
    public class get_processed_notification_tracker_when_tracker_does_not_exist : processed_notification_tracker_store_concern
    {       
        private static ProcessedNotificationTracker returnValue;

        private Establish ctx = () =>
            query.setup(x => x.SingleOrDefault()).Return((ProcessedNotificationTracker)null);

        private Because of = () =>
            returnValue = sut.GetProcessedNotificationTracker("typeName");

        private It should_return_tracker_with_type_name = () =>
            returnValue.TypeName.ShouldEqual("typeName");
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class track_notification_id : processed_notification_tracker_store_concern
    {
        private static ProcessedNotificationTracker tracker;

        private Establish ctx = () =>
        {
            tracker = fake.an<ProcessedNotificationTracker>();
            query.setup(x => x.SingleOrDefault()).Return(tracker);
        };

        private Because of = () =>
            sut.TrackMostRecentProcessedNotificationId(tracker, 1234);

        private It should_save_or_update = () =>
            session.received(x => x.SaveOrUpdate(tracker));

        private It should_track = () =>
            tracker.received(x => x.Track(1234));
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class delete_tracker : processed_notification_tracker_store_concern
    {
        private static ProcessedNotificationTracker tracker;

        private Establish ctx = () =>
        {
            tracker = new ProcessedNotificationTracker("typeName");
            query.setup(x => x.SingleOrDefault()).Return(tracker);
        };

        private Because of = () =>
            sut.DeleteTracker("typeName");

        private It should_delete_tracker = () =>
            session.received(x => x.Delete(Arg<ProcessedNotificationTracker>.Is.Equal(tracker)));
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class delete_non_existing_tracker : processed_notification_tracker_store_concern
    {
        private Establish ctx = () =>
            query.setup(x => x.SingleOrDefault()).Return((ProcessedNotificationTracker) null);

        private Because of = () =>
            spec.catch_exception(() =>
                sut.DeleteTracker("NonExisting"));

        private It should_not_throw_exception = () =>
            spec.exception_thrown.ShouldBeNull();
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class reset_tracker : processed_notification_tracker_store_concern
    {
        private static ProcessedNotificationTracker tracker;

        private Establish ctx = () =>
        {
            tracker = fake.an<ProcessedNotificationTracker>();

            query.setup(x => x.SingleOrDefault()).Return(tracker);
        };

        private Because of = () =>
            sut.ResetTracker("typeName");

        private It should_clear_error_message = () =>
            tracker.ErrorMessage.ShouldBeNull();

        private It should_reset_tracker = () =>
            tracker.received(x => x.Reset());

        private It should_save_or_update_tracker = () =>
            session.received(x => x.SaveOrUpdate(Arg<ProcessedNotificationTracker>.Is.Equal(tracker)));
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class reset_non_existing_tracker : processed_notification_tracker_store_concern
    {
        private Establish ctx = () =>
            query.setup(x => x.SingleOrDefault()).Return((ProcessedNotificationTracker) null);

        private Because of = () =>
            spec.catch_exception(() =>
                sut.ResetTracker("NonExisting"));

        private It should_not_save_or_update_tracker = () =>
            session.never_received(x => x.SaveOrUpdate(Arg<object>.Is.Anything));

        private It should_not_throw_exception = () =>
            spec.exception_thrown.ShouldBeNull();
    }
}