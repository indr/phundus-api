using Iesi.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.TestHelpers;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities.ArticleTests
{
    [TestFixture]
    public class ArticleNetStockTests : MockTestBase<Article>
    {
        #region SetUp/TearDown

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            _grossStockPropertyDef = new FieldDefinition(FieldDefinition.GrossStockId,
                                                                  "Bestand (Brutto)", FieldType.Integer);
        }

        #endregion

        private FieldDefinition _grossStockPropertyDef;

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

        [Test]
        public void Test()
        {
        }
    }
}
