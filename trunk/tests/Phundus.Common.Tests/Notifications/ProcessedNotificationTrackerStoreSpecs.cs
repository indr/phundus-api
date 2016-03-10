namespace Phundus.Common.Tests.Notifications
{
    using System;
    using System.Collections.Generic;
    using Common.Notifications;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Rhino.Mocks;

    public class processed_notification_tracker_store_concern : Observes<ProcessedNotificationTrackerStore>
    {
        protected static ITrackerRepository trackerRepository;

        private Establish ctx = () => { trackerRepository = depends.on<ITrackerRepository>(); };
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class when_store_gets_processed_notification_trackers : processed_notification_tracker_store_concern
    {
        private static IList<ProcessedNotificationTracker> returnValue;
        private static IList<ProcessedNotificationTracker> trackers = new List<ProcessedNotificationTracker>();

        private Establish ctx = () =>
        {
            trackers.Add(new ProcessedNotificationTracker("tracker1"));
            trackers.Add(new ProcessedNotificationTracker("tracker2"));
            trackerRepository.setup(x => x.GetAll()).Return(trackers);
        };

        private Because of = () =>
            returnValue = sut.GetProcessedNotificationTrackers();

        private It should_return_all_trackers = () =>
            returnValue.ShouldContain(trackers);
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class when_store_gets_processed_notification_tracker : processed_notification_tracker_store_concern
    {
        private static ProcessedNotificationTracker tracker;
        private static ProcessedNotificationTracker returnValue;

        private Establish ctx = () =>
        {
            tracker = fake.an<ProcessedNotificationTracker>();
            trackerRepository.setup(x => x.Find("typeName")).Return(tracker);
        };

        private Because of = () =>
            returnValue = sut.GetProcessedNotificationTracker("typeName");

        private It should_return_tracker = () =>
            returnValue.ShouldBeTheSameAs(tracker);
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class when_store_gets_processed_notification_tracker_when_tracker_does_not_exist :
        processed_notification_tracker_store_concern
    {
        private static ProcessedNotificationTracker returnValue;

        private Establish ctx = () =>
            trackerRepository.setup(x => x.Find("typeName")).Return((ProcessedNotificationTracker) null);

        private Because of = () =>
            returnValue = sut.GetProcessedNotificationTracker("typeName");

        private It should_return_tracker_with_type_name = () =>
            returnValue.TypeName.ShouldEqual("typeName");
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class when_store_tracks_exception_for_existing_tracker : processed_notification_tracker_store_concern
    {
        private static ProcessedNotificationTracker tracker;
        private static Exception exception = new Exception("Exception message");

        private Establish ctx = () =>
        {
            tracker = fake.an<ProcessedNotificationTracker>();
            trackerRepository.setup(x => x.Find("typeName")).Return(tracker);
        };

        private Because of = () =>
            sut.TrackException("typeName", exception);

        private It should_save_tracker = () =>
            trackerRepository.received(x => x.Save(tracker));

        private It should_track_exception = () =>
            tracker.received(x => x.Track(exception));
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class when_store_tracks_exception_for_non_existing_tracker : processed_notification_tracker_store_concern
    {
        private static Exception exception = new Exception("Exception message");

        private Establish ctx = () =>
            trackerRepository.setup(x => x.Find("typeName")).Return((ProcessedNotificationTracker) null);

        private Because of = () =>
            sut.TrackException("typeName", exception);

        private It should_save_or_update = () =>
            trackerRepository.received(x => x.Save(Arg<ProcessedNotificationTracker>.Matches(p =>
                !String.IsNullOrEmpty(p.ErrorMessage)
                && p.ErrorAtUtc > DateTime.MinValue)));
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class when_store_tracks_notification_id : processed_notification_tracker_store_concern
    {
        private static ProcessedNotificationTracker tracker;

        private Establish ctx = () =>
        {
            tracker = fake.an<ProcessedNotificationTracker>();
            trackerRepository.setup(x => x.Find("typeName")).Return(tracker);
        };

        private Because of = () =>
            sut.TrackMostRecentProcessedNotificationId(tracker, 1234);

        private It should_save_or_update = () =>
            trackerRepository.received(x => x.Save(tracker));

        private It should_track = () =>
            tracker.received(x => x.Track(1234));
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class when_store_deletes_tracker : processed_notification_tracker_store_concern
    {
        private static ProcessedNotificationTracker tracker;

        private Establish ctx = () =>
        {
            tracker = new ProcessedNotificationTracker("typeName");
            trackerRepository.setup(x => x.Find("typeName")).Return(tracker);
        };

        private Because of = () =>
            sut.DeleteTracker("typeName");

        private It should_delete_tracker = () =>
            trackerRepository.received(x => x.Remove(Arg<ProcessedNotificationTracker>.Is.Equal(tracker)));
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class when_store_deletes_non_existing_tracker : processed_notification_tracker_store_concern
    {
        private Establish ctx = () =>
            trackerRepository.setup(x => x.Find("typeName")).Return((ProcessedNotificationTracker) null);

        private Because of = () =>
            spec.catch_exception(() =>
                sut.DeleteTracker("NonExisting"));

        private It should_not_throw_exception = () =>
            spec.exception_thrown.ShouldBeNull();
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class when_store_resets_tracker : processed_notification_tracker_store_concern
    {
        private static ProcessedNotificationTracker tracker;

        private Establish ctx = () =>
        {
            tracker = fake.an<ProcessedNotificationTracker>();
            trackerRepository.setup(x => x.Find("typeName")).Return(tracker);
        };

        private Because of = () =>
            sut.ResetTracker("typeName");

        private It should_clear_error_message = () =>
            tracker.ErrorMessage.ShouldBeNull();

        private It should_reset_tracker = () =>
            tracker.received(x => x.Reset());

        private It should_save_or_update_tracker = () =>
            trackerRepository.received(x => x.Save(Arg<ProcessedNotificationTracker>.Is.Equal(tracker)));
    }

    [Subject(typeof (ProcessedNotificationTrackerStore))]
    public class when_store_resets_non_existing_tracker : processed_notification_tracker_store_concern
    {
        private Establish ctx = () =>
            trackerRepository.setup(x => x.Find("typeName")).Return((ProcessedNotificationTracker) null);

        private Because of = () =>
            spec.catch_exception(() =>
                sut.ResetTracker("NonExisting"));

        private It should_not_save_or_update_tracker = () =>
            trackerRepository.never_received(x => x.Save(Arg<ProcessedNotificationTracker>.Is.Anything));

        private It should_not_throw_exception = () =>
            spec.exception_thrown.ShouldBeNull();
    }
}