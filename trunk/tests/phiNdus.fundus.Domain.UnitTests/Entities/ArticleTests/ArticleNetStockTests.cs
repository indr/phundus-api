using Iesi.Collections.Generic;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities.ArticleTests
{
    [TestFixture]
    public class ArticleNetStockTests : ArticleTestBase
    {
        protected Article CreateSut()
        {
            StubFieldValues = new HashedSet<FieldValue>();
            return new Article(StubFieldValues);
        }

        protected HashedSet<FieldValue> StubFieldValues { get; set; }

        protected void AddGrossStockField(int amount)
        {
            StubFieldValues.Add(new FieldValue(GrossStockFieldDef, amount));
        }

        protected Article AddChild(Article parent)
        {
            var result = new Article();
            parent.AddChild(result);
            return result;
        }

        [Test]
        public void Test()
        {
        }
    }
}