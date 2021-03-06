﻿namespace Phundus.Specs.Features.Articles
{
    using System;
    using System.Linq;
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
                var alias = row.ContainsKey("Alias") ? row["Alias"] : null;
                if (Ctx.Articles.ContainsAlias(alias))
                    continue;

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

        [Given(@"I created an article in the default store with these values")]
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

        [When(@"I try to update the article details")]
        public void WhenITryToUpdateTheArticleDetails(Table table)
        {
            var row = table.Rows[0];
            App.UpdateArticleDetails(Ctx.Article.ArticleId, row["Name"], row["Brand"], row["Color"],
                Convert.ToInt32(row["Gross stock"]));
        }

        [Then(@"the article should equal")]
        public void ThenTheArticleShouldEqual(Table table)
        {
            Eventual.NoTestException(() =>
            {
                var article = App.GetArticle(Ctx.Article);
                table.CompareToInstance(article);
            });
        }

        [Then(@"the article ""(.*)"" should equal")]
        public void ThenTheArticleShouldEqual(string alias, Table table)
        {
            Eventual.NoTestException(() =>
            {
                var article = App.GetArticle(Ctx.Articles[alias]);
                table.CompareToInstance(article);
            });
        }

        [Then(@"I should see at least (.*) articles")]
        public void ThenIShouldSeeArticles(int number)
        {
            Assert.That(_articles, Is.Not.Null);
            Assert.That(_articles.Results.Count, Is.GreaterThanOrEqualTo(number));
        }

        [Given(@"I updated the article description:")]
        [When(@"I try to update the article description:")]
        public void WhenITryToUpdateTheArticleDescription(string multilineText)
        {
            App.UpdateArticleDescription(Ctx.Article.ArticleId, multilineText);
        }

        [Then(@"the article description is:")]
        public void ThenTheArticleDescriptionIs(string multilineText)
        {
            Eventual.NoTestException(() =>
            {
                var article = App.GetArticle(Ctx.Article);
                Assert.That(article.Description, Is.EqualTo(multilineText));
            });
        }

        [Given(@"I updated the article specification:")]
        [When(@"I try to update the article specification:")]
        public void WhenITryToUpdateTheArticleSpecification(string multilineText)
        {
            App.UpdateArticleSpecification(Ctx.Article.ArticleId, multilineText);
        }

        [Then(@"the article specification is:")]
        public void ThenTheArticleSpecificationIs(string multilineText)
        {
            Eventual.NoTestException(() =>
            {
                var article = App.GetArticle(Ctx.Article);
                Assert.That(article.Specification, Is.EqualTo(multilineText));
            });
        }

        [When(@"I tag a product with ""(.*)""")]
        public void WhenITagAProductWith(string p0)
        {
            App.TagProduct(Ctx.Article.OwnerId, Ctx.Article.ArticleId, p0);
        }

        [Then(@"the tag ""(.*)"" should be in the public list")]
        public void ThenTheTagShouldBeInThePublicList(string tagName)
        {
            Eventual.NoTestException(() =>
            {
                var tags = App.GetTags();
                Assert.That(tags.SingleOrDefault(p => p.Name == tagName), Is.Not.Null);
            });
        }

    }
}