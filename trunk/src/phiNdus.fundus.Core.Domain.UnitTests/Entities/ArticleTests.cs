using System;
using Castle.MicroKernel.Registration;
using Iesi.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using phiNdus.fundus.TestHelpers;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class ArticleTests : MockTestBase<Article>
    {
        protected override void RegisterDependencies(Castle.Windsor.IWindsorContainer container)
        {
            base.RegisterDependencies(container);

            StubPropertyDefinitionRepository = MockRepository.GenerateStub<IDomainPropertyDefinitionRepository>();

            container.Register(
                Component.For<IDomainPropertyDefinitionRepository>().Instance(StubPropertyDefinitionRepository));
        }

        protected IDomainPropertyDefinitionRepository StubPropertyDefinitionRepository { get; set; }

        protected override Article CreateSut()
        {
            StubPropertyValues = new HashedSet<DomainPropertyValue>();
            return new Article(StubPropertyValues);
        }

        private ISet<DomainPropertyValue> StubPropertyValues { get; set; }

        private readonly DomainPropertyDefinition _isReservablePropertyDef =
            new DomainPropertyDefinition(DomainPropertyDefinition.IsReservableId, "Reservierbar",
                                         DomainPropertyType.Boolean);

        private readonly DomainPropertyDefinition _isBorrowablePropertyDef =
            new DomainPropertyDefinition(DomainPropertyDefinition.IsBorrowableId, "Ausleihbar",
                                         DomainPropertyType.Boolean);
        
        private readonly DomainPropertyDefinition _amountPropertyDef =
                    new DomainPropertyDefinition(DomainPropertyDefinition.StockId, "Menge",
                                                 DomainPropertyType.Integer);

        private readonly DomainPropertyDefinition _pricePropertyDef =
                    new DomainPropertyDefinition(DomainPropertyDefinition.PriceId, "Preis",
                                                 DomainPropertyType.Decimal);

        [Test]
        public void Can_create()
        {
            var sut = new Article();
            Assert.That(sut, Is.Not.Null);
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
        public void Can_create_with_PropertyValues()
        {
            var propertyValues = new HashedSet<DomainPropertyValue>();
            var sut = new Article(propertyValues);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.PropertyValues, Is.SameAs(propertyValues));
        }

        [Test]
        public void GetIsBorrowable()
        {
            Assert.That(Sut.IsBorrowable, Is.False);
            StubPropertyValues.Add(new DomainPropertyValue(_isBorrowablePropertyDef, true));
            Assert.That(Sut.IsBorrowable, Is.True);
        }

        [Test]
        public void SetIsLendable()
        {
            StubPropertyDefinitionRepository.Stub(x => x.Get(_isBorrowablePropertyDef.Id)).Return(_isBorrowablePropertyDef);

            Assert.That(Sut.IsBorrowable, Is.False);
            Sut.IsBorrowable = true;
            Assert.That(Sut.IsBorrowable, Is.True);
        }

        [Test]
        public void GetIsReservable()
        {
            Assert.That(Sut.IsReservable, Is.False);
            StubPropertyValues.Add(new DomainPropertyValue(_isReservablePropertyDef, true));
            Assert.That(Sut.IsReservable, Is.True);
        }

        [Test]
        public void SetIsReservable()
        {
            StubPropertyDefinitionRepository.Stub(x => x.Get(_isReservablePropertyDef.Id)).Return(_isReservablePropertyDef);

            Assert.That(Sut.IsReservable, Is.False);
            Sut.IsReservable = true;
            Assert.That(Sut.IsReservable, Is.True);
        }

        [Test]
        public void GetStock()
        {
            Assert.That(Sut.Stock, Is.EqualTo(0));
            StubPropertyValues.Add(new DomainPropertyValue(_amountPropertyDef, 1));
            Assert.That(Sut.Stock, Is.EqualTo(1));
        }

        [Test]
        public void SetStock()
        {
            StubPropertyDefinitionRepository.Stub(x => x.Get(_amountPropertyDef.Id)).Return(_amountPropertyDef);

            Assert.That(Sut.Stock, Is.EqualTo(0));
            Sut.Stock = 1;
            Assert.That(Sut.Stock, Is.EqualTo(1));
        }

        [Test]
        public void GetPrice()
        {
            Assert.That(Sut.Price, Is.EqualTo(0.0d));
            StubPropertyValues.Add(new DomainPropertyValue(_pricePropertyDef, 1.1d));
            Assert.That(Sut.Price, Is.EqualTo(1.1d));
        }

        [Test]
        public void SetPrice()
        {
            StubPropertyDefinitionRepository.Stub(x => x.Get(_pricePropertyDef.Id)).Return(_pricePropertyDef);

            Assert.That(Sut.Price, Is.EqualTo(0.0d));
            Sut.Price = 1.1d;
            Assert.That(Sut.Price, Is.EqualTo(1.1d));
        }

        [Test]
        public void Is_derived_from_DomainObject()
        {
            var sut = new Article();
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut, Is.InstanceOf(typeof (DomainObject)));
        }
    }
}