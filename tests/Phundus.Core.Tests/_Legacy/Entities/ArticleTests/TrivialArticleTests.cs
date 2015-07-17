namespace Phundus.Core.Tests._Legacy.Entities.ArticleTests
{
    using System;
    using Core.Inventory.Articles.Model;
    using NUnit.Framework;

    [TestFixture]
    public class TrivialArticleTests : ArticleTestBase
    {
        protected Article CreateSut()
        {
           
            return new Article(1, "Dummy");
        }

        [Test]
        public void Create_sets_CreateDate()
        {
            Article sut = CreateSut();
            Assert.That(sut.CreateDate, Is.InRange(DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(1)));
        }

        [Test]
        public void SetPrice()
        {
            Article sut = CreateSut();
            
            Assert.That(sut.Price, Is.EqualTo(0.0d));
            sut.Price = 1.1m;
            Assert.That(sut.Price, Is.EqualTo(1.1d));
        }
    }
}