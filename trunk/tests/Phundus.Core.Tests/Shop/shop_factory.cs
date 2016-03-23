namespace Phundus.Tests.Shop
{
    using Common.Domain.Model;
    using developwithpassion.specifications.core;
    using developwithpassion.specifications.extensions;
    using Phundus.Shop.Model;
    using Phundus.Shop.Model.Products;

// ReSharper disable once InconsistentNaming
    public class shop_factory : factory_base
    {
        public shop_factory(ICreateFakes fake) : base(fake)
        {
        }

        public CartItem CartItem(Article product = null)
        {
            product = product ?? Product();
            var result = new CartItem(new CartItemId(), product);
            return result;
        }

        public Lessee Lessee(UserId userId = null)
        {
            userId = userId ?? new UserId();
            return Lessee(new LesseeId(userId));
        }

        public Lessee Lessee(LesseeId lesseeId)
        {
            var lessee = fake.an<Lessee>();
            lessee.setup(x => x.LesseeId).Return(lesseeId);
            lessee.setup(x => x.EmailAddress).Return("lessee@test.phundus.ch");
            lessee.setup(x => x.FullName).Return("The Lessee");
            return lessee;
        }

        public Lessor Lessor(LessorId lessorId = null)
        {
            var lessor = fake.an<Lessor>();
            lessor.setup(x => x.LessorId).Return(lessorId ?? new LessorId());
            return lessor;
        }

        public Manager Manager(UserId userId = null, string emailAddress = null)
        {
            userId = userId ?? new UserId();
            emailAddress = emailAddress ?? "manager@test.phundus.ch";
            return new Manager(userId, emailAddress, "The Manager");
        }

        public Order Order(Lessor lessor = null, Lessee lessee = null)
        {
            lessor = lessor ?? Lessor();
            lessee = lessee ?? Lessee();
            var order = fake.an<Order>();
            order.setup(x => x.OrderId).Return(new OrderId());
            order.setup(x => x.OrderShortId).Return(new OrderShortId(NextNumericId()));
            order.setup(x => x.Lessor).Return(lessor);
            order.setup(x => x.Lessee).Return(lessee);
            return order;
        }

        public Article Product(LessorId lessorId = null, string name = null, decimal? price = null)
        {
            var shortId = new ArticleShortId(NextNumericId());
            lessorId = lessorId ?? new LessorId();
            name = name ?? "The article " + shortId.Id;
            price = price ?? 7.0m;
            
            var article = new Article(shortId, new ArticleId(), Lessor(lessorId), new StoreId(),
                name, price.Value);
            return article;
        }
    }
}