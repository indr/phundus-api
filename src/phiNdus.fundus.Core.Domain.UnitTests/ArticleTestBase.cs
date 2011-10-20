using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using phiNdus.fundus.TestHelpers;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Domain.UnitTests
{
    public class ArticleTestBase : MockTestBase<Article>
    {
        #region Setup/Teardown

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            _grossStockFieldDef = new DomainPropertyDefinition(DomainPropertyDefinition.GrossStockId,
                                                               "Bestand (Brutto)", DomainPropertyType.Integer);
            _isBorrowableFieldDef = new DomainPropertyDefinition(DomainPropertyDefinition.IsBorrowableId,
                                                                 "Ausleihbar", DomainPropertyType.Boolean);
            _isReserverableFieldDef = new DomainPropertyDefinition(DomainPropertyDefinition.IsReservableId,
                                                                   "Reservierbar", DomainPropertyType.Boolean);

            FakeFieldDefRepository = GenerateAndRegisterStub<IDomainPropertyDefinitionRepository>();
            FakeOrderRepository = GenerateAndRegisterStub<IOrderRepository>();
            FakeContractRepository = GenerateAndRegisterStub<IContractRepository>();

            FakeFieldDefRepository.Expect(x => x.Get(_grossStockFieldDef.Id)).Return(_grossStockFieldDef);
            FakeFieldDefRepository.Expect(x => x.Get(_isBorrowableFieldDef.Id)).Return(_isBorrowableFieldDef);
            FakeFieldDefRepository.Expect(x => x.Get(_isReserverableFieldDef.Id)).Return(_isReserverableFieldDef);
        }

        #endregion

        private DomainPropertyDefinition _grossStockFieldDef;
        private DomainPropertyDefinition _isBorrowableFieldDef;
        private DomainPropertyDefinition _isReserverableFieldDef;

        protected IDomainPropertyDefinitionRepository FakeFieldDefRepository { get; set; }
        protected IOrderRepository FakeOrderRepository { get; set; }
        protected IContractRepository FakeContractRepository { get; set; }

        protected override Article CreateSut()
        {
            return new Article();
        }

        protected Article AddChild(bool isBorrowable = false, bool isReserverable = false)
        {
            var result = new Article();
            Sut.AddChild(result);
            Sut.IsBorrowable = isBorrowable;
            Sut.IsReservable = isReserverable;
            return result;
        }

        protected void SetAlreadyReservedAmount(int articleId, int amount)
        {
            FakeOrderRepository.Expect(x => x.SumReservedAmount(articleId)).Return(amount);
        }
    }
}