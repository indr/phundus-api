using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class DomainPropertyRepositoryTests : BaseTestFixture
    {
        [Test]
        public void Can_create()
        {
            new DomainPropertyRepository();
        }

        [Test]
        public void Can_find_all()
        {
            var sut = new DomainPropertyRepository();
            using (var uow = UnitOfWork.Start())
            {
                var actual = sut.FindAll();
                Assert.That(actual.Count, Is.EqualTo(1));
            }
        }
    }
}