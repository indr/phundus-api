namespace Phundus.Tests.Shop
{
    using System;
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

        public Article Article(Guid lessorId = default(Guid))
        {
            lessorId = lessorId == default(Guid) ? Guid.NewGuid() : lessorId;
            var shortId = new ArticleShortId(NextNumericId());
            var article = new Article(shortId, new ArticleId(), Lessor(new LessorId(lessorId)), new StoreId(),
                "The article " + shortId.Id, 7.0m);
            return article;
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
            return lessee;
        }

        public Lessor Lessor(LessorId lessorId = null)
        {
            var lessor = fake.an<Lessor>();
            lessor.setup(x => x.LessorId).Return(lessorId ?? new LessorId());
            return lessor;
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

        public Manager Manager(UserId userId = null, string emailAddress = null)
        {
            userId = userId ?? new UserId();
            emailAddress = emailAddress ?? "manager@test.phundus.ch";
            return new Manager(userId, emailAddress, "The Manager");
        }
    }
}