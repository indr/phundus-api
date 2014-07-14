namespace Phundus.Core.Inventory
{
    using System;
    using Entities;

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