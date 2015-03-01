namespace Phundus.Core.Tests.Inventory.Domain.Model.Management.Stock
{
    using Common.Domain.Model;
    using Core.Inventory.Domain.Model.Management;
    using Machine.Specifications;

    public class MultipleQuantityPeriodsConcern
    {
        protected static QuantityPeriods _qps1;
        protected static QuantityPeriods _qps2;
        protected static QuantityPeriods _resulting;

        private Establish ctx = () =>
        {
            _qps1 = new QuantityPeriods();
            _qps2 = new QuantityPeriods();
            _resulting = null;
        };
    }

    public class when_I_substract_two_empty_qpss : MultipleQuantityPeriodsConcern
    {
        private Because of = () => _resulting = _qps1.Sub(_qps2);

        public It should_be_empty = () => _resulting.IsEmpty.ShouldBeTrue();
    }

    public class when_I_substract_a_positive_qps_from_an_empty : MultipleQuantityPeriodsConcern
    {
        private Establish ctx = () => _qps2.Add(Period.FromTodayToTomorrow, 2);

        private Because of = () => _resulting = _qps1.Sub(_qps2);

        public It should_not_be_empty = () => _resulting.IsEmpty.ShouldBeFalse();

        public It should_have_negative_quantity_as_of =
            () => _resulting.QuantityAsOf(Period.FromTodayToTomorrow.FromUtc).ShouldEqual(-2);
    }
}