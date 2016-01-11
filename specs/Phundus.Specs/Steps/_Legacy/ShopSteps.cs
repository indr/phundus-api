namespace Phundus.Specs.Steps
{
    using System.Net.Http;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    using WatiN.Core;

    [Binding]
    public class ShopSteps : StepBase
    {
        [Given(@"mein Warenkorb ist leer")]
        public void GivenMeinWarenkorbIstLeer()
        {
            ApiCall("/carts", HttpMethod.Delete);
        }

        [When(@"ich wähle den Artikel (.*) aus")]
        public void WhenIchWahleDenArtikelAus(int p0)
        {
            articleId = p0;
            Browser.GoTo(BaseUrl + "/shop");
            var link = Browser.Link(Find.By("article-id", p0.ToString()));
            link.Click();
        }

        private int articleId = 0;

        [Then(@"muss der Artikel geöffnet sein")]
        public void ThenMussDerArtikelGeoffnetSein()
        {
            // <div id="10027" class="tab-pane active">
            Assert.That(Browser.Div(p => p.Id == articleId.ToString()).ClassName, Is.StringStarting("tab-pane"));
        }

        [Given(@"ich lege den Artikel mit der Id (.*) in den Warenkorb")]
        public void GivenIchLegeDenArtikelMitDerIdInDenWarenkorb(int p0)
        {
            Browser.GoTo(BaseUrl + "/shop");
            var link = Browser.Link(Find.By("article-id", p0.ToString()));
            link.Click();

            var button = Browser.Button(Find.ByValue("» in den Warenkorb"));
            button.Click();
        }

        [When(@"ich bestelle")]
        public void WhenIchBestelle()
        {
            Browser.GoTo(BaseUrl + "/cart/checkout");
            var button = Browser.Button(Find.ByValue("Bestellen"));
            button.Click();
        }

    }
}