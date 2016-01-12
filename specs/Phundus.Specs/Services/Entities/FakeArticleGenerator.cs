namespace Phundus.Specs.Services.Entities
{
    using System;
    using Assets;
    using ContentTypes;
    using TechTalk.SpecFlow;

    /// <summary>
    /// www.thinkcalculator.com/generator/product-name-generator.php
    /// </summary>
    [Binding]
    public class FakeArticleGenerator : FakeGeneratorBase<ArticleNameRow>
    {
        public FakeArticleGenerator() : base(Resources.ArticleNames)
        {
        }

        public Article NextArticle()
        {
            var record = GetNextRecord();
            var grossStock = Random.Next(1, 100);
            var price = Convert.ToDecimal(Random.NextDouble()*1000);
            return new Article
            {
                Name = record.Name,
                GrossStock = grossStock,
                Price = price
            };
        }
    }
}