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
        public void AngenommenMeinWarenkorbIstLeer()
        {
            ApiCall("/carts", HttpMethod.Delete);
        }

        [When(@"ich wähle den Artikel (.*) aus")]
        public void WennIchWahleDenArtikelAus(int p0)
        {
            articleId = p0;
            Browser.GoTo(BaseUrl + "/shop");
            var link = Browser.Link(Find.By("article-id", p0.ToString()));
            link.Click();
        }

        private int articleId = 0;

        [Then(@"muss der Artikel geöffnet sein")]
        public void DannMussDerArtikelGeoffnetSein()
        {
            // <div id="10027" class="tab-pane active">
            Assert.That(Browser.Div(p => p.Id == articleId.ToString()).ClassName, Is.StringStarting("tab-pane"));
        }

        [Given(@"ich lege den Artikel mit der Id (.*) in den Warenkorb")]
        public void AngenommenIchLegeDenArtikelMitDerIdInDenWarenkorb(int p0)
        {
            Browser.GoTo(BaseUrl + "/shop");
            var link = Browser.Link(Find.By("article-id", p0.ToString()));
            link.Click();

            var button = Browser.Button(Find.ByValue("» in den Warenkorb"));
            button.Click();
        }

        [When(@"ich bestelle")]
        public void WennIchBestelle()
        {
            Browser.GoTo(BaseUrl + "/cart/checkout");
            var button = Browser.Button(Find.ByValue("Bestellen"));
            button.Click();
        }

        [Then(@"ich sollte auf der Shopseite sein")]
        public void DannIchSollteAufDerShopseiteSein()
        {
            Assert.That(Browser.Url, Is.EqualTo(BaseUrl + "/shop"));
        }

    }
}