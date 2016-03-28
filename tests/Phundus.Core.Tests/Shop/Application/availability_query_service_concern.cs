namespace Phundus.Tests.Shop.Application
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Common.Domain.Model;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.Inventory.Application;
    using Phundus.Inventory.AvailabilityAndReservation.Model;

    public class availability_query_service_concern : Observes<AvailabilityQueryService>
    {
        protected static IAvailabilityService availabilityService;

        private Establish ctx = () => { availabilityService = depends.on<IAvailabilityService>(); };
    }

    [Subject(typeof (AvailabilityQueryService))]
    public class when_quantity_periods_are_not_available : availability_query_service_concern
    {
        private static ArticleId productId = new ArticleId();
        private static ICollection<QuantityPeriod> quantityPeriods = new Collection<QuantityPeriod>();
        private static AvailabilitiyInfo[] result;

        private Establish ctx = () =>
        {
            quantityPeriods.Add(new QuantityPeriod(new Period(new DateTime(2016, 04, 01), TimeSpan.FromDays(1)), 1));
            quantityPeriods.Add(new QuantityPeriod(new Period(new DateTime(2016, 04, 01), TimeSpan.FromDays(2)), 1));

            availabilityService.setup(x => x.GetAvailabilityDetails(productId)).Return(new List<Availability>());
        };

        private Because of = () =>
            result = sut.IsAvailable(productId, quantityPeriods).ToArray();

        private It should_return_quantity_periods_as_not_available = () =>
        {
            var first = result[0];
            var expected = new Period(new DateTime(2016, 04, 01), TimeSpan.FromDays(1));
            
            first.Period.FromUtc.ShouldEqual(expected.FromUtc);
            first.Period.ToUtc.ShouldEqual(expected.ToUtc);
            first.IsAvailable.ShouldBeFalse();
            first.ProductId.ShouldEqual(productId.Id);
        };
    }
}