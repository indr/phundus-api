namespace Phundus.Specs.Features.Articles
{
    using ContentTypes;
    using NUnit.Framework;
    using Services;
    using Services.Entities;
    using Steps;
    using TechTalk.SpecFlow;

    [Binding]
    public class ArticlesSteps : StepsBase
    {
        private QueryOkResponseContent<Article> _articles;
        private Files _files;

        public ArticlesSteps(App app, Ctx ctx, Files files) : base(app, ctx)
        {
            _files = files;
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
            Ctx.Article = App.CreateArticle(Ctx.User.UserId, null);
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

        [When(@"I try to upload an article image")]
        public void WhenITryToUploadAnArticleImage()
        {
            App.UploadArticleImage(Ctx.User, Ctx.Article, _files.GetNextImageFileName());
        }

    }
}