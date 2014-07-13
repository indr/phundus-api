namespace phiNdus.fundus.Domain.UnitTests.Entities.ArticleTests
{
    using System;
    using Iesi.Collections.Generic;
    using NUnit.Framework;
    using Phundus.Core.Entities;
    using Rhino.Mocks;

    [TestFixture]
    public class TrivialArticleTests : ArticleTestBase
    {
        protected Article CreateSut()
        {
            FakeFieldValues = new HashedSet<FieldValue>();
            return new Article(FakeFieldValues);
        }

        private ISet<FieldValue> FakeFieldValues { get; set; }


        [Test]
        public void Can_create()
        {
            var sut = new Article();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Can_create_with_FieldValues()
        {
            var fieldValues = new HashedSet<FieldValue>();
            var sut = new Article(fieldValues);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.FieldValues, Is.SameAs(fieldValues));
        }

        [Test]
        public void Can_create_with_Id_and_Version()
        {
            var sut = new Article(1, 2);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Id, Is.EqualTo(1));
            Assert.That(sut.Version, Is.EqualTo(2));
        }

        [Test]
        public void Create_sets_CreateDate()
        {
            Article sut = CreateSut();
            Assert.That(sut.CreateDate, Is.InRange(DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(1)));
        }

        [Test]
        public void GetCaption()
        {
            Article sut = CreateSut();
            Assert.That(sut.Caption, Is.EqualTo(""));
            FakeFieldValues.Add(new FieldValue(NameFieldDef, "Name of object"));
            Assert.That(sut.Caption, Is.EqualTo("Name of object"));
        }

        [Test]
        public void GetIsBorrowable()
        {
            Article sut = CreateSut();
            Assert.That(sut.IsBorrowable, Is.False);
            FakeFieldValues.Add(new FieldValue(IsBorrowableFieldDef, true));
            Assert.That(sut.IsBorrowable, Is.True);
        }

        [Test]
        public void GetIsReservable()
        {
            Article sut = CreateSut();
            Assert.That(sut.IsReservable, Is.False);
            FakeFieldValues.Add(new FieldValue(IsReservableFieldDef, true));
            Assert.That(sut.IsReservable, Is.True);
        }

        [Test]
        public void GetPrice()
        {
            Article sut = CreateSut();
            Assert.That(sut.Price, Is.EqualTo(0.0d));
            FakeFieldValues.Add(new FieldValue(PriceFieldDef, 1.1d));
            Assert.That(sut.Price, Is.EqualTo(1.1d));
        }

        [Test]
        public void SetCaption()
        {
            Article sut = CreateSut();
            FakeFieldDefRepository.Stub(x => x.ById(NameFieldDef.Id))
                .Return(NameFieldDef);

            Assert.That(sut.Caption, Is.EqualTo(""));
            sut.Caption = "Name of object";
            Assert.That(sut.Caption, Is.EqualTo("Name of object"));
        }

        [Test]
        public void SetIsLendable()
        {
            Article sut = CreateSut();
            FakeFieldDefRepository.Stub(x => x.ById(IsBorrowableFieldDef.Id)).Return(
                IsBorrowableFieldDef);

            Assert.That(sut.IsBorrowable, Is.False);
            sut.IsBorrowable = true;
            Assert.That(sut.IsBorrowable, Is.True);
        }

        [Test]
        public void SetIsReservable()
        {
            Article sut = CreateSut();
            FakeFieldDefRepository.Stub(x => x.ById(IsReservableFieldDef.Id)).Return(
                IsReservableFieldDef);

            Assert.That(sut.IsReservable, Is.False);
            sut.IsReservable = true;
            Assert.That(sut.IsReservable, Is.True);
        }

        [Test]
        public void SetPrice()
        {
            Article sut = CreateSut();
            FakeFieldDefRepository.Stub(x => x.ById(PriceFieldDef.Id)).Return(PriceFieldDef);

            Assert.That(sut.Price, Is.EqualTo(0.0d));
            sut.Price = 1.1d;
            Assert.That(sut.Price, Is.EqualTo(1.1d));
        }
    }
}