namespace Phundus.Common.Tests.Domain.Model
{
    using System;
    using Common.Domain.Model;
    using Machine.Specifications;

    public class quantity_periods_concern
    {
        protected static QuantityPeriods sut;

        private Cleanup cleanup = () =>
            DateTimeProvider.ResetToDefault();

        private Establish ctx = () =>
        {
            var provider = new InstrumentedDateTimeProvider(new DateTime(2016, 12, 01, 11, 35, 50));
            DateTimeProvider.Set(provider);

            sut = new QuantityPeriods();
        };

        protected static QuantityPeriod qP(int from, int to, int quantity = 1)
        {
            return new QuantityPeriod(
                new Period(DateTimeProvider.Today.AddDays(from - 1), TimeSpan.FromDays(to - from)),
                quantity);
        }
    }

    [Subject(typeof (QuantityPeriods))]
    public class when_creating_an_empty_quantity_periods : quantity_periods_concern
    {
        private It should_have_empty_string_representation = () =>
            sut.ToString().ShouldEqual("QuantityPeriods (0):");

        private It should_have_zero_intervals = () =>
            sut.Intervals.Count.ShouldEqual(0);
    }

    [Subject(typeof (QuantityPeriods))]
    public class when_adding_a_period_to_an_empty_quantity_periods : quantity_periods_concern
    {
        private Because of = () =>
            sut.Add(qP(1, 2, 99));

        private It should_have_1_interval = () =>
            sut.Intervals.Count.ShouldEqual(1);

        private It should_have_string_representation = () =>
            sut.ToString().ShouldEqual(@"QuantityPeriods (1):
01.12.2016 00:00:00 - 02.12.2016 00:00:00 | 99");
    }

    [Subject(typeof (QuantityPeriods))]
    public class when_adding_two_quantity_periods_without_intersection : quantity_periods_concern
    {
        private Establish ctx = () =>
            sut.Add(qP(3, 4, 11));

        private Because of = () =>
            sut.Add(qP(1, 2, 12));

        private It should_have_string_representation = () =>
            sut.ToString().ShouldEqual(@"QuantityPeriods (2):
01.12.2016 00:00:00 - 02.12.2016 00:00:00 | 12
03.12.2016 00:00:00 - 04.12.2016 00:00:00 | 11");

        private It should_have_two_intervals = () =>
            sut.Intervals.Count.ShouldEqual(2);
    }

    [Subject(typeof (QuantityPeriods))]
    public class when_adding_an_exact_match : quantity_periods_concern
    {
        private Establish ctx = () =>
            sut.Add(qP(1, 2, 3));

        private Because of = () =>
            sut.Add(qP(1, 2, 2));

        private It should_have_string_representation = () =>
            sut.ToString().ShouldEqual(@"QuantityPeriods (1):
01.12.2016 00:00:00 - 02.12.2016 00:00:00 | 5");
    }

    [Subject(typeof(QuantityPeriods))]
    public class when_adding_start_touching : quantity_periods_concern
    {
        private Establish ctx = () =>
            sut.Add(qP(1, 3));

        private Because of = () =>
            sut.Add(qP(3, 4));

        private It should_have_string_representation = () =>
            sut.ToString().ShouldEqual(@"QuantityPeriods (2):
01.12.2016 00:00:00 - 03.12.2016 00:00:00 | 1
03.12.2016 00:00:00 - 04.12.2016 00:00:00 | 1");
    }

    [Subject(typeof (QuantityPeriods))]
    public class when_adding_start_inside : quantity_periods_concern
    {
        private Establish ctx = () =>
            sut.Add(qP(1, 3));

        private Because of = () =>
            sut.Add(qP(2, 4));

        private It should_have_string_representation = () =>
            sut.ToString().ShouldEqual(@"QuantityPeriods (3):
01.12.2016 00:00:00 - 02.12.2016 00:00:00 | 1
02.12.2016 00:00:00 - 03.12.2016 00:00:00 | 2
03.12.2016 00:00:00 - 04.12.2016 00:00:00 | 1");
    }

    [Subject(typeof (QuantityPeriods))]
    public class when_adding_inside_start_touching : quantity_periods_concern
    {
        private Establish ctx = () =>
            sut.Add(qP(1, 4));

        private Because of = () =>
            sut.Add(qP(1, 2));

        private It should_have_string_representation = () =>
            sut.ToString().ShouldEqual(@"QuantityPeriods (2):
01.12.2016 00:00:00 - 02.12.2016 00:00:00 | 2
02.12.2016 00:00:00 - 04.12.2016 00:00:00 | 1");
    }

    [Subject(typeof(QuantityPeriods))]
    public class when_adding_enclosing_start_touching : quantity_periods_concern
    {
        private Establish ctx = () =>
            sut.Add(qP(1, 2));

        private Because of = () =>
            sut.Add(qP(1, 4));

        private It should_have_string_representation = () =>
            sut.ToString().ShouldEqual(@"QuantityPeriods (2):
01.12.2016 00:00:00 - 02.12.2016 00:00:00 | 2
02.12.2016 00:00:00 - 04.12.2016 00:00:00 | 1");
    }

    [Subject(typeof(QuantityPeriods))]
    public class when_adding_enclosing : quantity_periods_concern
    {
        private Establish ctx = () =>
            sut.Add(qP(2, 3));

        private Because of = () =>
            sut.Add(qP(1, 4));

        private It should_have_string_representation = () =>
            sut.ToString().ShouldEqual(@"QuantityPeriods (3):
01.12.2016 00:00:00 - 02.12.2016 00:00:00 | 1
02.12.2016 00:00:00 - 03.12.2016 00:00:00 | 2
03.12.2016 00:00:00 - 04.12.2016 00:00:00 | 1");
    }

    [Subject(typeof(QuantityPeriods))]
    public class when_adding_enclosing_end_touching : quantity_periods_concern
    {
        private Establish ctx = () =>
            sut.Add(qP(2, 3));

        private Because of = () =>
            sut.Add(qP(1, 3));

        private It should_have_string_representation = () =>
            sut.ToString().ShouldEqual(@"QuantityPeriods (2):
01.12.2016 00:00:00 - 02.12.2016 00:00:00 | 1
02.12.2016 00:00:00 - 03.12.2016 00:00:00 | 2");
    }

    [Subject(typeof(QuantityPeriods))]
    public class when_adding_inside : quantity_periods_concern
    {
        private Establish ctx = () =>
            sut.Add(qP(1, 4));

        private Because of = () =>
            sut.Add(qP(2, 3));

        private It should_have_string_representation = () =>
            sut.ToString().ShouldEqual(@"QuantityPeriods (3):
01.12.2016 00:00:00 - 02.12.2016 00:00:00 | 1
02.12.2016 00:00:00 - 03.12.2016 00:00:00 | 2
03.12.2016 00:00:00 - 04.12.2016 00:00:00 | 1");
    }

    [Subject(typeof(QuantityPeriods))]
    public class when_adding_inside_end_touching : quantity_periods_concern
    {
        private Establish ctx = () =>
            sut.Add(qP(1, 4));

        private Because of = () =>
            sut.Add(qP(2, 4));

        private It should_have_string_representation = () =>
            sut.ToString().ShouldEqual(@"QuantityPeriods (2):
01.12.2016 00:00:00 - 02.12.2016 00:00:00 | 1
02.12.2016 00:00:00 - 04.12.2016 00:00:00 | 2");
    }

    [Subject(typeof(QuantityPeriods))]
    public class when_adding_inside_end_inside : quantity_periods_concern
    {
        private Establish ctx = () =>
            sut.Add(qP(2, 4));

        private Because of = () =>
            sut.Add(qP(1, 3));

        private It should_have_string_representation = () =>
          sut.ToString().ShouldEqual(@"QuantityPeriods (3):
01.12.2016 00:00:00 - 02.12.2016 00:00:00 | 1
02.12.2016 00:00:00 - 03.12.2016 00:00:00 | 2
03.12.2016 00:00:00 - 04.12.2016 00:00:00 | 1");
    }

    [Subject(typeof(QuantityPeriods))]
    public class when_adding_end_touching : quantity_periods_concern
    {
        private Establish ctx = () =>
            sut.Add(qP(2, 4));

        private Because of = () =>
            sut.Add(qP(1, 2));

        private It should_have_string_representation = () =>
          sut.ToString().ShouldEqual(@"QuantityPeriods (2):
01.12.2016 00:00:00 - 02.12.2016 00:00:00 | 1
02.12.2016 00:00:00 - 04.12.2016 00:00:00 | 1");
    }
}