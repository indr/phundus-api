namespace Phundus.Core.Tests.Inventory.Domain.Model.Management.Stock
{
    using System;
    using Common.Domain.Model;
    using Core.Inventory.Domain.Model.Management;
    using Machine.Specifications;

    public class SingleQuantityPeriodsConcern
    {
        protected static QuantityPeriods _qps = new QuantityPeriods();
    }

    public class when_I_create_a_qps
    {
        private static QuantityPeriods _qps;

        private Because of = () => _qps = new QuantityPeriods();

        public It should_be_empty = () => _qps.IsEmpty.ShouldBeTrue();

        public It should_not_have_quantity_as_of = () => _qps.HasQuantityAsOf(DateTime.UtcNow, 1).ShouldBeFalse();

        public It should_have_quantity_as_of_zero = () => _qps.QuantityAsOf(DateTime.UtcNow).ShouldEqual(0);
    }

    public class when_I_add_a_qp_to_an_empty_qps : SingleQuantityPeriodsConcern
    {
        private static Period _period = Period.FromTodayToTomorrow;
        private static int _quantity = 2;

        private static QuantityPeriod _qp = new QuantityPeriod(_period, _quantity);

        private Because of = () => _qps.Add(_qp);

        public It should_not_be_empty = () => _qps.IsEmpty.ShouldBeFalse();

        public It should_have_quantity_as_of_one_sec_before_period_from_equal_0 =
            () => _qps.QuantityAsOf(_period.FromUtc.AddSeconds(-1)).ShouldEqual(0);

        public It should_have_quantity_as_of_period_start_equal_2 =
            () => _qps.QuantityAsOf(_period.FromUtc).ShouldEqual(2);

        public It should_have_quantity_as_of_period_end_equal_2 =
            () => _qps.QuantityAsOf(_period.ToUtc).ShouldEqual(2);

        public It should_have_quantity_as_of_one_sec_after_period_to_equal_0 =
            () => _qps.QuantityAsOf(_period.ToUtc.AddSeconds(1)).ShouldEqual(0);

        public It should_not_have_quantity_one_sec_before_period_from =
            () => _qps.HasQuantityAsOf(_period.FromUtc.AddSeconds(-1), _quantity).ShouldBeFalse();

        public It should_have_quantity_at_period_from =
            () => _qps.HasQuantityAsOf(_period.FromUtc, _quantity).ShouldBeTrue();

        public It should_not_have_more_than_quantity_at_period_from =
            () => _qps.HasQuantityAsOf(_period.FromUtc, _quantity + 1).ShouldBeFalse();

        public It should_have_less_than_quantity_at_period_from =
            () => _qps.HasQuantityAsOf(_period.FromUtc, _quantity - 1).ShouldBeTrue();

        public It should_have_quantity_at_period_to =
            () => _qps.HasQuantityAsOf(_period.ToUtc, _quantity).ShouldBeTrue();

        public It should_not_have_more_than_quantity_at_period_to =
            () => _qps.HasQuantityAsOf(_period.ToUtc, _quantity + 1).ShouldBeFalse();

        public It should_have_less_than_quantity_at_period_to =
            () => _qps.HasQuantityAsOf(_period.ToUtc, _quantity - 1).ShouldBeTrue();

        public It should_not_have_quantity_one_sec_after_period_to =
            () => _qps.HasQuantityAsOf(_period.ToUtc.AddSeconds(1), _quantity).ShouldBeFalse();
    }

    public class when_I_add_a_qp_to_existing_quantity : SingleQuantityPeriodsConcern
    {
        private Establish ctx = () => _qps.Add(new QuantityPeriod(Period.FromTodayToTomorrow, 2));

        private Because of = () => _qps.Add(new QuantityPeriod(Period.FromTodayToTomorrow, 3));

        public It should_have_summed_quantity_in_period =
            () => _qps.HasQuantityAsOf(Period.FromTodayToTomorrow.FromUtc.AddSeconds(1), 5).ShouldBeTrue();
    }
}