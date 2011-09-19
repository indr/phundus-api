using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class ArticleRepositoryTests : BaseTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            Sut = IoC.Resolve<IArticleRepository>();
        }

        protected IArticleRepository Sut { get; set; }

        [Test]
        public void Can_save_and_load()
        {
            var articleId = 0;
            var article = new Article();
           

            using (var uow = UnitOfWork.Start())
            {
                article.Caption = "Artikel";

                Sut.Save(article);
                articleId = article.Id;
                uow.TransactionalFlush();
            }

            using (var uow = UnitOfWork.Start())
            {
                article = Sut.Get(articleId);

                Assert.That(article, Is.Not.Null);
                Assert.That(article.Caption, Is.EqualTo("Artikel"));
            }
        }
    }
}