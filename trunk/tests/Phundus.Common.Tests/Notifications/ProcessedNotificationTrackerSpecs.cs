namespace Phundus.Common.Tests.Notifications
{
    using System;
    using Common.Notifications;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class processed_notification_tracker_concern : Observes<ProcessedNotificationTracker>
    {
        protected static InstrumentedDateTimeProvider dateTime;

        private Cleanup cleanup = () =>
            DateTimeProvider.ResetToDefault();

        private Establish ctx = () =>
        {
            dateTime = new InstrumentedDateTimeProvider(DateTime.UtcNow.AddDays(1));
            DateTimeProvider.Set(dateTime);
        };
    }

    [Subject(typeof (ProcessedNotificationTracker))]
    public class track_notification_id : processed_notification_tracker_concern
    {
        private Establish ctx = () =>
            sut_setup.run(sut =>
            {
                sut.Track(new InvalidOperationException());
                dateTime.HoursPassed();
            });

        private Because of = () =>
            sut.Track(1234);

        private It should_clear_error_message = () =>
            sut.ErrorMessage.ShouldBeNull();

        private It should_clear_error_at_utc = () =>
            sut.ErrorAtUtc.ShouldBeNull();

        private It should_set_last_processing_at_utc = () =>
            sut.MostRecentProcessedAtUtc.ShouldEqual(dateTime.UtcNow);

        private It should_set_most_recent_processed_notification_id = () =>
            sut.MostRecentProcessedNotificationId.ShouldEqual(1234);
    }

    [Subject(typeof (ProcessedNotificationTracker))]
    public class track_exception : processed_notification_tracker_concern
    {
        private static DateTime mostRecentProcssedAtUtc;

        private Establish ctx = () =>
            sut_setup.run(sut =>
            {
                mostRecentProcssedAtUtc = dateTime.UtcNow;
                sut.Track(1);
                dateTime.HoursPassed();
            });

        private Because of = () =>
            sut.Track(new InvalidOperationException("Error message"));

        private It should_not_touch_most_recent_processed_at_utc = () =>
            sut.MostRecentProcessedAtUtc.ShouldEqual(mostRecentProcssedAtUtc);

        private It should_not_touch_most_recent_processed_notification_id = () =>
            sut.MostRecentProcessedNotificationId.ShouldEqual(1);

        private It should_set_error_message = () =>
            sut.ErrorMessage.ShouldEqual("Error message");

        private It should_set_error_at_utc = () =>
            sut.ErrorAtUtc.ShouldEqual(DateTimeProvider.UtcNow);
    }

    [Subject(typeof (ProcessedNotificationTracker))]
    public class reset_tracker : processed_notification_tracker_concern
    {
        private Establish ctx = () =>
            sut_setup.run(sut =>
            {
                sut.Track(1);
                sut.Track(new InvalidOperationException());
                dateTime.HoursPassed();
            });

        private Because of = () =>
            sut.Reset();

        private It should_clear_error_message = () =>
            sut.ErrorMessage.ShouldBeNull();

        private It should_clear_error_at = () =>
            sut.ErrorAtUtc.ShouldBeNull();

        private It should_set_most_recent_processed_at_utc = () =>
            sut.MostRecentProcessedAtUtc.ShouldEqual(dateTime.UtcNow);

        private It should_set_most_recent_processed_notification_id_to_0 = () =>
            sut.MostRecentProcessedNotificationId.ShouldEqual(0);
    }
}