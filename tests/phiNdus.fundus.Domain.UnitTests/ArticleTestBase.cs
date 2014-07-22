namespace phiNdus.fundus.Domain.UnitTests
{
    using NUnit.Framework;
    using Phundus.Core.InventoryCtx;
    using Phundus.Core.InventoryCtx.Model;
    using Phundus.Core.InventoryCtx.Repositories;
    using Phundus.Core.ReservationCtx;
    using Phundus.Core.ReservationCtx.Repositories;
    using Rhino.Mocks;
    using TestHelpers.TestBases;

    public class ArticleTestBase : UnitTestBase<Article>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            FakeFieldDefRepository = GenerateAndRegisterStub<IFieldDefinitionRepository>();
            FakeOrderRepository = GenerateAndRegisterStub<IOrderRepository>();

            FakeFieldDefRepository.Expect(x => x.ById(GrossStockFieldDef.Id)).Return(GrossStockFieldDef);
            FakeFieldDefRepository.Expect(x => x.ById(IsBorrowableFieldDef.Id)).Return(IsBorrowableFieldDef);
            FakeFieldDefRepository.Expect(x => x.ById(IsReservableFieldDef.Id)).Return(IsReservableFieldDef);
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