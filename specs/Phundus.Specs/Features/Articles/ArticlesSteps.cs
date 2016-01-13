namespace Phundus.Specs.Features.Articles
{
    using System.Net;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class ArticlesSteps : StepsBase
    {
        private QueryOkResponseContent<Article> _articles;

        public ArticlesSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [When(@"I create an article in my store")]
        public void WhenICreateAnArticleInMyStore()
        {
            Ctx.Article = App.CreateArticle(Ctx.User, null);
        }

        [Given(@"I created an article in my store")]
        public void GivenICreatedAnArticleInMyStore(Table table)
        {
            if (table.RowCount == 0)
            {
                Ctx.Article = App.CreateArticle(Ctx.User, null);
            }
            else foreach (var row in table.Rows)
            {
                Ctx.Article = App.CreateArticle(Ctx.User, row);    
            }
            
        }

        [When(@"I try to query all my articles")]
        public void WhenITryToQueryAllMyArticles()
        {
            _articles = App.QueryArticlesByUser(Ctx.User);
        }

        [Then(@"I should see (.*) articles")]
        public void ThenIShouldSeeArticles(int number)
        {
            Assert.That(_articles, Is.Not.Null);
            Assert.That(_articles.Results.Count, Is.EqualTo(number));
        }
    }
}