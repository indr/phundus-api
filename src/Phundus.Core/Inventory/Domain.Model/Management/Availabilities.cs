namespace Phundus.Core.Inventory.Domain.Model.Management
{
    using System.Collections.Generic;
    using Catalog;
    using Common.Domain.Model;
    using IdentityAndAccess.Domain.Model.Organizations;

    public class Availabilities : EventSourcedEntity
    {
        private OrganizationId _organizationId;
        private ArticleId _articleId;
        private StockId _stockId;

        private readonly QuantitiesAsOf _quantities = new QuantitiesAsOf();
        private readonly QuantityPeriods _qps = new QuantityPeriods();

        public ICollection<QuantityAsOf> Quantities { get { return _quantities.Quantities; } }

        public void When(StockCreated e)
        {
            _organizationId = new OrganizationId(e.OrganizationId);
            _articleId = new ArticleId(e.ArticleId);
            _stockId = new StockId(e.StockId);
        }

        public void When(QuantityAvailableChanged e)
        {
            if (e.Change > 0)
            {
                _qps.Add(e.Period, e.Change);
                _quantities.IncreaseAsOf(e.Change, e.FromUtc);
                _quantities.DecreaseAsOf(e.Change, e.ToUtc);
            }
            else
            {
                _qps.Sub(e.Period, e.Change);
                _quantities.DecreaseAsOf(e.Change * -1, e.FromUtc);
                _quantities.IncreaseAsOf(e.Change * -1, e.ToUtc);
            }
        }

        public ICollection<IDomainEvent> GenerateMutatingEvents(QuantityPeriods availabilities)
        {
            var result = new List<IDomainEvent>();


            var difference = availabilities.Sub(_qps);

            foreach (var each in difference.Quantities)
            {
                result.Add(
                    new QuantityAvailableChanged(_organizationId, _articleId, _stockId, each.Period, each.Quantity));
            }

            return result;
        }

        protected override void When(IDomainEvent e)
        {
            When((dynamic)e);
        }

        protected void When(DomainEvent e)
        {
            // Fallback
        }

        public override string ToString()
        {
            return "Availabilities [Qps=" + _qps + "]";
        }
    }
}