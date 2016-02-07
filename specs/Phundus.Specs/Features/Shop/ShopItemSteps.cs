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
        private ShopItem _shopItem;

        public ShopItemSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"I try to get the shop item details")]
        public void WhenITryToGetTheShopItemDetails()
        {
            _shopItem = App.GetShopItemDetails(Ctx.Article.ArticleId);
        }

        [Then(@"the shop item should equal")]
        public void ThenTheShopItemShouldEqual(Table table)
        {
            table.CompareToInstance(_shopItem);
        }

        [Then(@"the shop item should have (.*) document")]
        public void ThenTheShopItemShouldHaveDocument(int p0)
        {
            Assert.That(_shopItem.Documents, Has.Count.EqualTo(p0));
        }

        [Then(@"the shop item should have (.*) image")]
        public void ThenTheShopItemShouldHaveImage(int p0)
        {
            Assert.That(_shopItem.Images, Has.Count.EqualTo(p0));
        }
    }
}