namespace Phundus.Core.Tests.Inventory.Domain.Model.Management.Stock
{
    using Common.Domain.Model;
    using Core.Inventory.Domain.Model.Management;
    using Machine.Specifications;

    public class QuantityPeriodConcern
    {
        
    }

    public class when_I_create_a_qp : QuantityPeriodConcern
    {
        private static Period _period = Period.FromTodayToTomorrow;
        private static int _quantity = 5;

        private static QuantityPeriod _qp;

        private Because of = () => _qp = new QuantityPeriod(_period, _quantity);

        public It is_in_period_one_sec_before_period_from_should_be_false =
            () => _qp.IsInPeriod(_period.FromUtc.AddSeconds(-1)).ShouldBeFalse();

        public It is_in_period_at_period_from_should_be_true =
            () => _qp.IsInPeriod(_period.FromUtc).ShouldBeTrue();

        public It is_in_period_at_period_to_should_be_true =
            () => _qp.IsInPeriod(_period.ToUtc).ShouldBeTrue();

        public It is_in_period_one_sec_after_period_to_should_be_false =
            () => _qp.IsInPeriod(_period.ToUtc.AddSeconds(1)).ShouldBeFalse();
    }
}