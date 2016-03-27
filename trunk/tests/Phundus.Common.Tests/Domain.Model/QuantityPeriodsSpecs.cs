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

        protected static QuantityPeriod qP(string from, string to, int quantity = 1)
        {
            return new QuantityPeriod(Convert.ToDateTime(from), Convert.ToDateTime(to), quantity);
        }
    }

    public class quantity_periods_calulcation_concern : quantity_periods_concern
    {
        protected static string result;

        private Because of = () =>
            result = sut.ToString();

        protected static void add(string start, string end, int quantity = 1)
        {
            sut.Add(qP(start.TrimEnd('Z'), end.TrimEnd('Z'), quantity));
        }

        protected static void add(int start, int end, int quantity = 1)
        {
            sut.Add(qP(start, end, quantity));
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

    [Subject(typeof (QuantityPeriods))]
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

    [Subject(typeof (QuantityPeriods))]
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

    [Subject(typeof (QuantityPeriods))]
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

    [Subject(typeof (QuantityPeriods))]
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

    [Subject(typeof (QuantityPeriods))]
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

    [Subject(typeof (QuantityPeriods))]
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

    [Subject(typeof (QuantityPeriods))]
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

    [Subject(typeof (QuantityPeriods))]
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

    [Subject(typeof (QuantityPeriods))]
    public class multiple_intersections : quantity_periods_calulcation_concern
    {
        private Establish ctx = () =>
        {
            add(1, 9, 2);
            add(2, 7, -1);
            add(3, 8, -1);
        };

        private It should_result = () =>
            result.ShouldEqual(@"QuantityPeriods (5):
01.12.2016 00:00:00 - 02.12.2016 00:00:00 | 2
02.12.2016 00:00:00 - 03.12.2016 00:00:00 | 1
03.12.2016 00:00:00 - 07.12.2016 00:00:00 | 0
07.12.2016 00:00:00 - 08.12.2016 00:00:00 | 1
08.12.2016 00:00:00 - 09.12.2016 00:00:00 | 2");
    }

    [Subject(typeof (QuantityPeriods))]
    public class multiple_intersections_with_time : quantity_periods_calulcation_concern
    {
        /* {"productId":"cba6cc1d-e9cf-407a-a1d5-d3823c8cd094","items":[
         * {"fromUtc":"2016-03-25T22:44:21Z","toUtc":"2016-03-31T22:44:21Z","quantity":1},
         * {"fromUtc":"2016-03-26T22:46:30Z","toUtc":"2016-03-31T22:46:30Z","quantity":1}]} */

        private Establish ctx = () =>
        {
            add("2016-03-01T22:00:00Z", "2017-03-01T22:00:00Z", 2);
            add("2016-03-25T22:44:21Z", "2016-03-31T22:44:21Z", -1);
            add("2016-03-26T22:46:30Z", "2016-03-31T22:46:30Z", -1);
        };

        private It should_result = () =>
            result.ShouldEqual(@"QuantityPeriods (5):
01.03.2016 22:00:00 - 25.03.2016 22:44:21 | 2
25.03.2016 22:44:21 - 26.03.2016 22:46:30 | 1
26.03.2016 22:46:30 - 31.03.2016 22:44:21 | 0
31.03.2016 22:44:21 - 31.03.2016 22:46:30 | 1
31.03.2016 22:46:30 - 01.03.2017 22:00:00 | 2");
    }
}