namespace phiNdus.fundus.Domain.UnitTests
{
    using NUnit.Framework;
    using Phundus.Core.Inventory.Articles.Model;
    using Phundus.Core.Shop.Orders.Repositories;
    using Rhino.Mocks;
    using TestHelpers.TestBases;

    public class ArticleTestBase : UnitTestBase<Article>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();


            FakeOrderRepository = GenerateAndRegisterStub<IOrderRepository>();
        }

        #endregion

        protected IOrderRepository FakeOrderRepository { get; set; }


        protected void SetAlreadyReservedAmount(int articleId, int amount)
        {
            FakeOrderRepository.Expect(x => x.SumReservedAmount(articleId)).Return(amount);
        }
    }
}