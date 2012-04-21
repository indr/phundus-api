using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using phiNdus.fundus.TestHelpers;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Domain.UnitTests
{
    public class ArticleTestBase : UnitTestBase<Article>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            FakeFieldDefRepository = GenerateAndRegisterStub<IFieldDefinitionRepository>();
            FakeOrderRepository = GenerateAndRegisterStub<IOrderRepository>();
            FakeContractRepository = GenerateAndRegisterStub<IContractRepository>();

            FakeFieldDefRepository.Expect(x => x.Get(GrossStockFieldDef.Id)).Return(GrossStockFieldDef);
            FakeFieldDefRepository.Expect(x => x.Get(IsBorrowableFieldDef.Id)).Return(IsBorrowableFieldDef);
            FakeFieldDefRepository.Expect(x => x.Get(IsReservableFieldDef.Id)).Return(IsReservableFieldDef);
        }

        #endregion

        protected readonly FieldDefinition GrossStockFieldDef = new FieldDefinition(FieldDefinition.GrossStockId,
                                                                                    "Bestand (Brutto)", DataType.Integer);

        protected readonly FieldDefinition IsBorrowableFieldDef = new FieldDefinition(FieldDefinition.IsBorrowableId,
                                                                                      "Ausleihbar", DataType.Boolean);

        protected readonly FieldDefinition IsReservableFieldDef = new FieldDefinition(FieldDefinition.IsReservableId,
                                                                                      "Reservierbar", DataType.Boolean);

        protected readonly FieldDefinition NameFieldDef =
            new FieldDefinition(FieldDefinition.CaptionId, "Name",
                                DataType.Text);

        protected readonly FieldDefinition PriceFieldDef =
            new FieldDefinition(FieldDefinition.PriceId, "Preis",
                                DataType.Decimal);

        protected IFieldDefinitionRepository FakeFieldDefRepository { get; set; }
        protected IOrderRepository FakeOrderRepository { get; set; }
        protected IContractRepository FakeContractRepository { get; set; }

        protected Article AddChild(Article parent, bool isBorrowable = false, bool isReserverable = false)
        {
            var result = new Article();
            parent.AddChild(result);
            parent.IsBorrowable = isBorrowable;
            parent.IsReservable = isReserverable;
            return result;
        }

        protected void SetAlreadyReservedAmount(int articleId, int amount)
        {
            FakeOrderRepository.Expect(x => x.SumReservedAmount(articleId)).Return(amount);
        }
    }
}