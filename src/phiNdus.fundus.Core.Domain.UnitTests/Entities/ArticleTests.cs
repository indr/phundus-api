using Iesi.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.TestHelpers;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class ArticleTests : MockTestBase<Article>
    {
        protected override Article CreateSut()
        {
            StubPropertyValues = new HashedSet<DomainPropertyValue>();
            return new Article(StubPropertyValues);
        }

        private ISet<DomainPropertyValue> StubPropertyValues { get; set; }

        private readonly DomainPropertyDefinition _isReservablePropertyDef =
            new DomainPropertyDefinition(DomainPropertyDefinition.ReservierbarId, "Reservierbar",
                                         DomainPropertyType.Boolean);

        private readonly DomainPropertyDefinition _isLendablePropertyDef =
            new DomainPropertyDefinition(DomainPropertyDefinition.AusleihbarId, "Ausleihbar",
                                         DomainPropertyType.Boolean);
        
        private readonly DomainPropertyDefinition _amountPropertyDef =
                    new DomainPropertyDefinition(DomainPropertyDefinition.MengeId, "Menge",
                                                 DomainPropertyType.Integer);

        private readonly DomainPropertyDefinition _pricePropertyDef =
                    new DomainPropertyDefinition(DomainPropertyDefinition.PreisId, "Preis",
                                                 DomainPropertyType.Decimal);

        [Test]
        public void Can_create()
        {
            var sut = new Article();
            Assert.That(sut, Is.Not.Null);
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
        public void GetIsLendable()
        {
            Assert.That(Sut.IsLendable, Is.False);
            StubPropertyValues.Add(new DomainPropertyValue(_isLendablePropertyDef, true));
            Assert.That(Sut.IsLendable, Is.True);
        }

        [Test]
        public void GetIsReservable()
        {
            Assert.That(Sut.IsReservable, Is.False);
            StubPropertyValues.Add(new DomainPropertyValue(_isReservablePropertyDef, true));
            Assert.That(Sut.IsReservable, Is.True);
        }

        [Test]
        public void GetAmount()
        {
            Assert.That(Sut.Amount, Is.EqualTo(0));
            StubPropertyValues.Add(new DomainPropertyValue(_amountPropertyDef, 1));
            Assert.That(Sut.Amount, Is.EqualTo(1));
        }

        [Test]
        public void GetPrice()
        {
            Assert.That(Sut.Price, Is.EqualTo(0.0d));
            StubPropertyValues.Add(new DomainPropertyValue(_pricePropertyDef, 1.1d));
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