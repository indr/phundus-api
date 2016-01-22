namespace Phundus.Specs.Services.Entities
{
    using System;
    using Specs.Assets;
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

        public Article NextArticle(TableRow row)
        {
            var record = GetNextRecord();
            var grossStock = Random.Next(1, 100);
            var price = Convert.ToDecimal(Random.NextDouble()*1000);
            var result = new Article
            {
                Name = record.Name,
                GrossStock = grossStock,
                Price = price,
                PublicPrice = price,
                MemberPrice = (decimal?)null
            };

            return SetPropertiesFromRow(result, row);
        }

        private Article SetPropertiesFromRow(Article result, TableRow row)
        {
            if (row == null)
                return result;

            if (row.ContainsKey("Name"))
                result.Name = row["Name"];
            if (row.ContainsKey("Stock"))
                result.GrossStock = Convert.ToInt32(row["Stock"]);
            if (row.ContainsKey("Gross stock"))
                result.GrossStock = Convert.ToInt32(row["Gross stock"]);
            if (row.ContainsKey("Public price"))
                result.PublicPrice = Convert.ToDecimal(row["Public price"]);
            if (row.ContainsKey("Member price"))
                result.MemberPrice = Convert.ToDecimal(row["Member price"]);
            return result;
        }
    }
}