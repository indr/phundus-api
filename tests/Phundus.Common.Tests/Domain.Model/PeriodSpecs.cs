using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phundus.Common.Tests.Domain.Model
{
    using Common.Domain.Model;
    using Machine.Specifications;

    [Subject(typeof(Period))]
    public class when_comparing_two_different_periods
    {
        private Establish ctx = () =>
        {
            _period1 = Period.FromTodayToTomorrow;
            _period2 = Period.FromTodayToTomorrow.ShiftDays(1);
        };

        private static Period _period1;
        private static Period _period2;

        private Because of = () =>
        {
            _result = Equals(_period1, _period2);
        };

        public It should_be_false = () => _result.ShouldBeFalse();
        private static bool _result;
    }

    [Subject(typeof(Period))]
    public class when_comparing_two_same_periods
    {
        private Establish ctx = () =>
        {
            _period1 = Period.FromTodayToTomorrow;
            _period2 = Period.FromTodayToTomorrow;
        };

        private static Period _period1;
        private static Period _period2;

        private Because of = () =>
        {
            _result = Equals(_period1, _period2);
        };

        public It should_be_true = () => _result.ShouldBeTrue();
        private static bool _result;
    }
}
