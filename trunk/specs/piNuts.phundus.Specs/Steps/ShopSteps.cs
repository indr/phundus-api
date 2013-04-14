using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WatiN.Core;

namespace piNuts.phundus.Specs.Steps
{
    [Binding]
    public class ShopSteps : StepBase
    {
        [Given(@"mein Warenkorb ist leer")]
        public void AngenommenMeinWarenkorbIstLeer()
        {
            ApiCall("/basket/clear");
            //Browser.GoTo(BaseUrl + "/cart");
            //if (Browser.TableCell(Find.ByClass("no-data")).Exists)
            //    return;

            //var link = Browser.Links.FirstOrDefault(p => p.InnerHtml == "Entfernen");
            //while (link != null && link.Exists)
            //{
            //    link.Click();
            //    link = Browser.Links.FirstOrDefault(p => p.InnerHtml == "Entfernen");
            //}
            //Browser.GoTo(BaseUrl + "/cart");
            //var td = Browser.TableCell(Find.ByClass("no-data"));
            //Assert.That(td.Exists, Is.True);
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
            Assert.That(Browser.Url, Is.EqualTo("http://" + BaseUrl + "/shop"));
        }

    }
}