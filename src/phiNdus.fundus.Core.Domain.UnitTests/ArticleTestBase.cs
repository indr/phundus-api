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

            _grossStockFieldDef = new FieldDefinition(FieldDefinition.GrossStockId,
                                                               "Bestand (Brutto)", DataType.Integer);
            _isBorrowableFieldDef = new FieldDefinition(FieldDefinition.IsBorrowableId,
                                                                 "Ausleihbar", DataType.Boolean);
            _isReserverableFieldDef = new FieldDefinition(FieldDefinition.IsReservableId,
                                                                   "Reservierbar", DataType.Boolean);

            FakeFieldDefRepository = GenerateAndRegisterStub<IFieldDefinitionRepository>();
            FakeOrderRepository = GenerateAndRegisterStub<IOrderRepository>();
            FakeContractRepository = GenerateAndRegisterStub<IContractRepository>();

            FakeFieldDefRepository.Expect(x => x.Get(_grossStockFieldDef.Id)).Return(_grossStockFieldDef);
            FakeFieldDefRepository.Expect(x => x.Get(_isBorrowableFieldDef.Id)).Return(_isBorrowableFieldDef);
            FakeFieldDefRepository.Expect(x => x.Get(_isReserverableFieldDef.Id)).Return(_isReserverableFieldDef);
        }

        #endregion

        private FieldDefinition _grossStockFieldDef;
        private FieldDefinition _isBorrowableFieldDef;
        private FieldDefinition _isReserverableFieldDef;

        protected IFieldDefinitionRepository FakeFieldDefRepository { get; set; }
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