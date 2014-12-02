﻿namespace Phundus.Core.Tests._Legacy
{
    using Core.Inventory.Domain.Model.Catalog;
    using Core.Shop.Domain.Model.Ordering;
    using NUnit.Framework;
    using Rhino.Mocks;

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