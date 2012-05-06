using System;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Domain.Inventory
{
    public class GrossStockCalculator
    {
        private readonly Article _article;

        public GrossStockCalculator(Article article)
        {
            _article = article;
        }

        public int At(DateTime day)
        {
            return _article.GrossStock;
        }
    }
}