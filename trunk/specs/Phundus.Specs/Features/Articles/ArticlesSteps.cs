namespace Phundus.Specs.Features.Articles
{
    using System;
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Steps;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class ArticlesSteps : AppStepsBase
    {
        private QueryOkResponseContent<Article> _articles;

        public ArticlesSteps(App app, Ctx ctx) : base(app, ctx)
        {
        }

        [Given(@"I created an article in my store")]
        public void GivenICreatedAnArticleInMyStore()
        {
            Ctx.Article = App.CreateArticle(Ctx.User.UserId);
        }

        [Given(@"I created these articles in my store")]
        public void GivenICreatedTheseArticlesInMyStore(Table table)
        {
            foreach (var row in table.Rows)
            {
                Ctx.Article = App.CreateArticle(Ctx.User.UserId, row);
            }
        }

        [Given(@"I created an article in the default store")]
        public void GivenICreatedAnArticleInTheDefaultStore()
        {
            Ctx.Article = App.CreateArticle(Ctx.Organization.OrganizationId);
        }

        [Given(@"with these organization articles")]
        public void GivenWithTheseOrganizationArticles(Table table)
        {
            foreach (var row in table.Rows)
            {
                Ctx.Article = App.CreateArticle(Ctx.Organization.OrganizationId, row);
                if (row.ContainsKey("Alias"))
                    Ctx.Articles[row["Alias"]] = Ctx.Article;
            }
        }

        [When(@"I create an article in my store")]
        public void WhenICreateAnArticleInMyStore()
        {
            Ctx.Article = App.CreateArticle(Ctx.User.UserId);
        }

        [When(@"I create an article in the default store")]
        public void WhenICreateAnArticleInTheDefaultStore()
        {
            Ctx.Article = App.CreateArticle(Ctx.Organization.OrganizationId);
        }

        [When(@"I create an article in the default store with these values")]
        public void WhenICreateAnArticleInTheDefaultStoreWithTheseValues(Table table)
        {
            var row = table.Rows[0];
            Ctx.Article = App.CreateArticle(Ctx.Organization.OrganizationId, row);

            if (row.ContainsKey("Alias"))
                Ctx.Articles[row["Alias"]] = Ctx.Article;
        }

        [When(@"I try to change the price to (.*) and (.*)")]
        public void WhenITryToChangeThePriceToAnd(decimal publicPrice, decimal memberPrice)
        {
            App.ChangeArticlePrice(Ctx.Article.ArticleId, publicPrice, memberPrice);
        }

        [When(@"I try to query all my articles")]
        public void WhenITryToQueryAllMyArticles()
        {
            _articles = App.QueryArticlesByUser(Ctx.User);
        }

        [When(@"I try to query all the organizations articles")]
        public void WhenITryToQueryAllTheOrganizationsArticles()
        {
            _articles = App.QueryArticlesByOrganization(Ctx.Organization);
        }

        [Then(@"the article should equal")]
        public void ThenTheArticleShouldEqual(Table table)
        {
            var article = App.GetArticle(Ctx.Article);
            table.CompareToInstance(article);
        }

        [Then(@"the article ""(.*)"" should equal")]
        public void ThenTheArticleShouldEqual(string alias, Table table)
        {
            var article = App.GetArticle(Ctx.Articles[alias]);
            table.CompareToInstance(article);
        }

        [Then(@"I should see (.*) articles")]
        public void ThenIShouldSeeArticles(int number)
        {
            Assert.That(_articles, Is.Not.Null);
            Assert.That(_articles.Results.Count, Is.EqualTo(number));
        }
    }
}