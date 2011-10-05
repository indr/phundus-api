using NUnit.Framework;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.UnitTests.Services
{
    [TestFixture]
    public class OrderServiceTests : BaseTestFixture
    {
        #region SetUp/TearDown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            FakeUnitOfWork = GenerateAndRegisterStubUnitOfWork();

            Sut = new ArticleService();
        }

        #endregion

        protected ArticleService Sut { get; set; }

        protected IUnitOfWork FakeUnitOfWork { get; set; }
        protected IOrderRepository FakeOrderRepo { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<IOrderRepository>() == null)
            {
                FakeOrderRepo = GenerateAndRegisterStub<IOrderRepository>();
            }
        }

        [Test]
        public void Can_create()
        {
            var sut = new OrderService();
            Assert.That(sut, Is.Not.Null);
        }
    }
}