namespace Phundus.Specs.Services.Entities
{
    using Assets;
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

        public FakeArticle NextArticle()
        {
            var record = GetNextRecord();
            var grossStock = Random.Next(0, 100);
            return new FakeArticle(record.Name, grossStock);
        }
    }

    public class FakeArticle
    {
        public FakeArticle(string name, int grossStock)
        {
            Name = name;
            GrossStock = grossStock;
        }

        public string Name { get; set; }
        public int GrossStock { get; set; }
    }
}