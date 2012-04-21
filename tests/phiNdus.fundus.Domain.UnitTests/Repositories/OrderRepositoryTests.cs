using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Repositories;

namespace phiNdus.fundus.Core.Domain.UnitTests.Repositories
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