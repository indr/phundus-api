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


            using (var uow = UnitOfWork.Start())
            {
                var fromSession = UnitOfWork.CurrentSession.Get<Article>(articleId);

                Assert.That(fromSession, Is.Not.Null);
                // TODO: Assert.That(fromSession, Is.EqualTo(article));
                Assert.That(fromSession.Id, Is.EqualTo(articleId));
            }
        }
    }
}