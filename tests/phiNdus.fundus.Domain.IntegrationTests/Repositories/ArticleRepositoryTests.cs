using System;
using NUnit.Framework;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class ArticleRepositoryTests : DomainComponentTestBase<IArticleRepository>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Sut = new ArticleRepository();
        }

        [Test]
        public void Can_save_and_load()
        {
            var articleId = 0;
            var article = new Article();
           

            using (var uow = UnitOfWork.Start())
            {
                var organization = new Organization();
                organization.Name = Guid.NewGuid().ToString("N");
                UnitOfWork.CurrentSession.Save(organization);
                article.Organization = organization;
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