namespace Phundus.Specs.Features.Shop
{
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class ShopItemSteps : AppStepsBase
    {
        public ShopItemSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"I try to get the shop item details")]
        public void WhenITryToGetTheShopItemDetails()
        {
            //_shopItem = App.GetShopItemDetails(Ctx.Article.ArticleId);
        }

        [Then(@"the shop item should equal")]
        public void ThenTheShopItemShouldEqual(Table table)
        {
            Eventual.NoTestException(() =>
            {
                var product = App.GetShopItemDetails(Ctx.Article.ArticleId);
                table.CompareToInstance(product);
            });
        }

        [Then(@"the shop item should have (.*) document")]
        public void ThenTheShopItemShouldHaveDocument(int number)
        {
            Eventual.NoTestException(() =>
            {
                var product = App.GetShopItemDetails(Ctx.Article.ArticleId);
                Assert.That(product.Documents, Has.Count.EqualTo(number));
            });
        }

        [Then(@"the shop item should have (.*) image")]
        public void ThenTheShopItemShouldHaveImage(int number)
        {
            Eventual.NoTestException(() =>
            {
                var product = App.GetShopItemDetails(Ctx.Article.ArticleId);
                Assert.That(product.Images, Has.Count.EqualTo(number));
            });
        }
    }
}