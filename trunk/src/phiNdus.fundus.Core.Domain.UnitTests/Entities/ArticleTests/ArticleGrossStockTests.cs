using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.TestHelpers;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities.ArticleTests
{
    [TestFixture]
    public class ArticleGrossStockTests : MockTestBase<Article>
    {
        private DomainPropertyDefinition _grossStockPropertyDef;

        [SetUp]
        public override void  Setup()
        {
 	        base.Setup();

            _grossStockPropertyDef = new DomainPropertyDefinition(DomainPropertyDefinition.GrossStockId, "Bestand (Brutto)", DomainPropertyType.Integer);
        }

        protected override Article CreateSut()
        {
            StubPropertyValues = new HashedSet<DomainPropertyValue>();
            return new Article(StubPropertyValues);
        }

        protected HashedSet<DomainPropertyValue> StubPropertyValues { get; set; }

        protected void AddGrossStockProperty(int amount)
        {
            StubPropertyValues.Add(new DomainPropertyValue(_grossStockPropertyDef, amount));
        }

        protected Article AddChild()
        {
            var result = new Article();
            Sut.AddChild(result);
            return result;
        }

        /// <summary>
        /// Kein definierter Bestand, keine Kindelemente => Bruttobestand = 1
        /// </summary>
        [Test]
        public void Get_without_children_and_without_attached_property_returns_1()
        {
            var actual = Sut.GrossStock;
            Assert.That(actual, Is.EqualTo(1));
        }

        [Test]
        public void Get_without_children_and_attached_property_returns_propertyValue()
        {
            AddGrossStockProperty(100);

            var actual = Sut.GrossStock;
            Assert.That(actual, Is.EqualTo(100));
        }

        [Test]
        public void Get_with_children_and_attached_property_throws()
        {
            AddGrossStockProperty(100);
            AddChild();

            Assert.Throws<IllegalAttachedFieldException>(delegate { object current = Sut.GrossStock; });
        }

        [Test]
        public void Get_with_children_returns_sum_of_childrens_GrossStock()
        {
            var child1 = MockRepository.GenerateMock<Article>();
            var child2 = MockRepository.GenerateMock<Article>();
            Sut.AddChild(child1);
            Sut.AddChild(child2);

            child1.Expect(x => x.GrossStock).Return(10);
            child2.Expect(x => x.GrossStock).Return(20);

            var actual = Sut.GrossStock;
            Assert.That(actual, Is.EqualTo(30));
        }
    }
}
