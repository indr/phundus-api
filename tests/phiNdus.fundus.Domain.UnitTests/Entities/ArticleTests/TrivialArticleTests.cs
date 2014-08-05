namespace phiNdus.fundus.Domain.UnitTests.Entities.ArticleTests
{
    using System;
    using Iesi.Collections.Generic;
    using NUnit.Framework;
    using Phundus.Core.Inventory.Model;
    using Rhino.Mocks;

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
        public void SetCaption()
        {
            Article sut = CreateSut();
            
            Assert.That(sut.Caption, Is.Null);
            sut.Caption = "Name of object";
            Assert.That(sut.Caption, Is.EqualTo("Name of object"));
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