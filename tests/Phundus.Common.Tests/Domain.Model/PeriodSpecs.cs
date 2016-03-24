namespace Phundus.Common.Tests.Domain.Model
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class period_concern : Observes
    {
        protected static DateTime theFromUtc;
        protected static DateTime theToUtc;

        protected static Period sut;

        private Establish ctx = () =>
        {
            theFromUtc = new DateTime(2010, 11, 1);
            theToUtc = new DateTime(2010, 11, 2);
        };
    }

    [Subject(typeof (Period))]
    public class when_creating_a_period_with_to_date : period_concern
    {
        private Because of = () =>
            sut = new Period(theFromUtc, theToUtc);

        private It should_have_from_utc = () =>
            sut.FromUtc.ShouldEqual(theFromUtc);

        private It should_have_to_utc = () =>
            sut.ToUtc.ShouldEqual(theToUtc);
    }

    [Subject(typeof (Period))]
    public class when_creating_a_period_with_timespan : period_concern
    {
        private Because of = () =>
            sut = new Period(theFromUtc, TimeSpan.FromDays(2));

        private It should_have_from_utc = () =>
            sut.FromUtc.ShouldEqual(theFromUtc);

        private It should_have_to_utc = () =>
            sut.ToUtc.ShouldEqual(theFromUtc.Add(TimeSpan.FromDays(2)));
    }

    [Subject(typeof (Period))]
    public class when_creating_a_period_with_smaller_from_than_to : period_concern
    {
        private Establish ctx = () =>
        {
            theFromUtc = DateTime.Now.AddDays(1);
            theToUtc = DateTime.Now.AddDays(-1);
        };

        private Because of = () => spec.catch_exception(() =>
            sut = new Period(theFromUtc, theToUtc));

        private It should_throw_argument_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<ArgumentException>();

        private It should_throw_exception_with_message = () =>
            spec.exception_thrown.Message.ShouldEqual("The from date must be less or equal the to date.");
    }

    [Subject(typeof (Period))]
    public class when_comparing_two_periods_with_same_values : period_concern
    {
        private static Period theOtherPeriod;
        private static bool result;

        private Establish ctx = () =>
        {
            sut = new Period(theFromUtc, theToUtc);
            theOtherPeriod = new Period(theFromUtc, theToUtc);
        };

        private Because of = () =>
            result = Equals(sut, theOtherPeriod);

        private It should_be_equal = () =>
            result.ShouldBeTrue();
    }

    [Subject(typeof (Period))]
    public class when_creating_from_now_with_days : period_concern
    {
        private static int days = 2;

        private Because of = () =>
            sut = Period.FromNow(days);

        private It should_have_from_utc = () =>
            sut.FromUtc.ShouldBeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));

        private It should_have_to_utc = () =>
            sut.ToUtc.ShouldBeCloseTo(DateTime.UtcNow.AddDays(days), TimeSpan.FromMinutes(1));
    }

    [Subject(typeof (Period))]
    public class two_periods_not_intersecting : period_concern
    {
        private static Period result;

        private Because of = () =>
            result = Period.FromNow(2).Intersection(new Period(DateTime.UtcNow.AddDays(99), TimeSpan.FromDays(1)));

        private It should_not_intersect_and_return_null = () =>
            result.ShouldBeNull();
    }

    [Subject(typeof (Period))]
    public class two_periods_intersecting_at_start : period_concern
    {
        private static Period result;

        private Because of = () =>
            result = Period.FromNow(2).Intersection(new Period(DateTime.UtcNow.AddDays(-2), TimeSpan.FromDays(1)));

        private It should_not_intersect_and_return_null;
    }
}