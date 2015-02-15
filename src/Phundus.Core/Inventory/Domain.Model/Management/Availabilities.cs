namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Collections.Generic;
    using Catalog;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class Availabilities
    {
        private OrganizationId _organizationId;
        private ArticleId _articleId;
        private StockId _stockId;

        private readonly QuantitiesAsOf _quantities = new QuantitiesAsOf();
        private readonly QuantityPeriods _quantityPeriods = new QuantityPeriods();

        public Availabilities(OrganizationId organizationId, ArticleId articleId, StockId stockId)
        {
            _organizationId = organizationId;
            _articleId = articleId;
            _stockId = stockId;
        }

        public ICollection<QuantityAsOf> Quantities { get { return _quantities.Quantities; } }

        public void When(QuantityAvailableChanged e)
        {
            if (e.Change > 0)
            {
                _quantities.IncreaseAsOf(e.Change, e.FromUtc);
                _quantities.DecreaseAsOf(e.Change, e.ToUtc);
            }
            else
            {
                _quantities.DecreaseAsOf(e.Change * -1, e.FromUtc);
                _quantities.IncreaseAsOf(e.Change * -1, e.ToUtc);
            }
        }

        public ICollection<IDomainEvent> GenerateMutatingEvents(QuantityPeriods availabilities)
        {
            var result = new List<IDomainEvent>();


            var difference = availabilities.Sub(_quantityPeriods);

            foreach (var each in difference.Quantities)
            {
                result.Add(
                    new QuantityAvailableChanged(_organizationId, _articleId, _stockId, each.Period, each.Quantity));
            }

            return result;
        }
    }
}