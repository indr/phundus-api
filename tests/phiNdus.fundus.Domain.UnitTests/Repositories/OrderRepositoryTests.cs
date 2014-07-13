using NUnit.Framework;
using phiNdus.fundus.Domain.Repositories;

namespace phiNdus.fundus.Domain.UnitTests.Repositories
{
    [TestFixture]
    public class OrderRepositoryTests
    {
        [Test]
        public void Can_create()
        {
            IOrderRepository sut = new OrderRepository();
            Assert.That(sut, Is.Not.Null);
        }
    }
}