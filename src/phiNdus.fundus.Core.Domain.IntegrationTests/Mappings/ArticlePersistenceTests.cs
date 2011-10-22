using System;
using System.Threading;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Mappings
{
    [TestFixture]
    public class ArticlePersistenceTests : BaseTestFixture
    {
        [Test]
        public void Can_save_and_load()
        {
            var article = new Article();
            var articleId = 0;

            using (var uow = UnitOfWork.Start())
            {
                UnitOfWork.CurrentSession.Save(article);
                articleId = article.Id;
                uow.TransactionalFlush();
            }


            using (UnitOfWork.Start())
            {
                var fromSession = UnitOfWork.CurrentSession.Get<Article>(articleId);

                Assert.That(fromSession, Is.Not.Null);
                // TODO: Assert.That(fromSession, Is.EqualTo(article));
                Assert.That(fromSession.Id, Is.EqualTo(articleId));
            }
        }

        [Test]
        public void Update_does_not_update_CreateDate()
        {
            var article = new Article();
            var articleId = 0;
            var createDate = article.CreateDate;
            createDate = createDate.AddMilliseconds(-createDate.Millisecond);



            using (var uow = UnitOfWork.Start())
            {
                UnitOfWork.CurrentSession.Save(article);
                articleId = article.Id;
                uow.TransactionalFlush();
            }

            Thread.Sleep(2 * 1000);

            using (var uow = UnitOfWork.Start())
            {
                article = UnitOfWork.CurrentSession.Get<Article>(articleId);
                article.Caption = "Updated";
                UnitOfWork.CurrentSession.SaveOrUpdate(article);
                uow.TransactionalFlush();
            }

            using (UnitOfWork.Start())
            {
                article = UnitOfWork.CurrentSession.Get<Article>(articleId);
                Assert.That(article, Is.Not.Null);
                Assert.That(article.Version, Is.EqualTo(2));
                Assert.That(article.CreateDate.ToString(), Is.EqualTo(createDate.ToString()));
            }

        }
    }
}