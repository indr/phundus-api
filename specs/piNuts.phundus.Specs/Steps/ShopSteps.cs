using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WatiN.Core;

namespace piNuts.phundus.Specs.Steps
{
    using System.Net.Http;

    [Binding]
    public class ShopSteps : StepBase
    {
        [Given(@"mein Warenkorb ist leer")]
        public void AngenommenMeinWarenkorbIstLeer()
        {
            ApiCall("/carts", HttpMethod.Delete);
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