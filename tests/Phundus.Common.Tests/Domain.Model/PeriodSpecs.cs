namespace Phundus.Common.Tests.Domain.Model
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;

    public class period_concern
    {
        protected static DateTime theFromUtc;
        protected static DateTime theToUtc;

        private Establish ctx = () =>
        {
            theFromUtc = new DateTime(2010, 11, 1);
            theToUtc = new DateTime(2010, 11, 2);
        };

        protected static Period sut;
    }

    [Subject(typeof (Period))]
    public class when_creating_a_period : period_concern
    {
        private Because of = () => sut = new Period(theFromUtc, theToUtc);

        private It should_have_from_utc = () => sut.FromUtc.ShouldEqual(theFromUtc);

        private It should_have_to_utc = () => sut.ToUtc.ShouldEqual(theToUtc);
    }

    [Subject(typeof (Period))]
    public class when_creating_a_period_with_smaller_from_than_to : period_concern
    {
        private static Exception caught;

        private Establish ctx = () =>
        {
            theFromUtc = DateTime.Now.AddDays(1);
            theToUtc = DateTime.Now.AddDays(-1);
        };

        private Because of = () => caught = Catch.Exception(() => sut = new Period(theFromUtc, theToUtc));

        private It should_throw_argument_exception = () => caught.ShouldBeOfExactType<ArgumentException>();

        private It should_throw_exception_with_message =
            () => caught.Message.ShouldEqual("The from date must be less or equal the to date.");
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
        private Because of = () => result = Equals(sut, theOtherPeriod);

        private It should_be_equal = () => result.ShouldBeTrue();
    }
}