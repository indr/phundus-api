using System;
using NUnit.Framework;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class FieldDefinitionRepositoryTests : DomainComponentTestBase<IFieldDefinitionRepository>
    {
        [Test]
        public void Can_find_all()
        {
            Sut = new FieldDefinitionRepository();

            using (UnitOfWork.Start())
            {
                var actual = Sut.FindAll();

                Assert.That(actual, Has.Count.GreaterThanOrEqualTo(11));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Name").And.Property("DataType").EqualTo(DataType.Text));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Preis").And.Property("DataType").EqualTo(DataType.Decimal));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Verfügbar").And.Property("DataType").EqualTo(DataType.Boolean));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Bestand (Brutto)").And.Property("DataType").EqualTo(DataType.Integer));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Bestand (Netto)").And.Property("DataType").EqualTo(DataType.Integer));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Erfassungsdatum").And.Property("DataType").EqualTo(DataType.DateTime));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Reservierbar").And.Property("DataType").EqualTo(DataType.Boolean));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Ausleihbar").And.Property("DataType").EqualTo(DataType.Boolean));
            }
        }
    }
}