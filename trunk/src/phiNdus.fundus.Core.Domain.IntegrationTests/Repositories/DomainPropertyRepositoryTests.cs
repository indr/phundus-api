using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class DomainPropertyRepositoryTests : BaseTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            Sut = IoC.Resolve<IDomainPropertyRepository>();
        }

        protected IDomainPropertyRepository Sut { get; set; }

        [Test]
        public void Can_find_all()
        {
            using (var uow = UnitOfWork.Start())
            {
                var actual = Sut.FindAll();

                Assert.That(actual, Has.Count.EqualTo(5));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Name").And.Property("Type").EqualTo(DomainPropertyType.Text));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Preis").And.Property("Type").EqualTo(DomainPropertyType.Decimal));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Verfügbar").And.Property("Type").EqualTo(DomainPropertyType.Boolean));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Menge").And.Property("Type").EqualTo(DomainPropertyType.Integer));
                Assert.That(actual, Has.Some.Property("Name").EqualTo("Erfassungsdatum").And.Property("Type").EqualTo(DomainPropertyType.DateTime));
            }
        }
    }
}