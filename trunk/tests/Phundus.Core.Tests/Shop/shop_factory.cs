namespace Phundus.Tests.Shop
{
    using System;
    using Common.Domain.Model;
    using developwithpassion.specifications.core;
    using developwithpassion.specifications.extensions;
    using Machine.Fakes;
    using Phundus.Shop.Model;
    using Phundus.Shop.Orders.Model;

    public class shop_factory : factory_base
    {
        public shop_factory(ICreateFakes fake) : base(fake)
        {
        }

        public Article Article(Guid lessorId = default(Guid))
        {
            lessorId = lessorId == default(Guid) ? Guid.NewGuid() : lessorId;

            var articleId = new ArticleShortId(NextNumericId());
            var article = fake.an<Article>();
            article.setup(x => x.Id).Return(articleId.Id);
            article.setup(x => x.ArticleId).Return(new ArticleId());
            article.setup(x => x.ArticleShortId).Return(articleId);
            article.setup(x => x.LessorId).Return(new LessorId(lessorId));
            article.setup(x => x.Caption).Return("The article " + articleId.Id);
            return article;
        }

        public Lessee Lessee()
        {
            var lessee = fake.an<Lessee>();
            lessee.setup(x => x.LesseeId).Return(new LesseeId());
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
            order.setup(x => x.ShortOrderId).Return( new ShortOrderId(NextNumericId()));
            order.setup(x => x.Lessor).Return(lessor);
            order.setup(x => x.Lessee).Return(lessee);
            return order;
        }

        public Owner Owner(OwnerId ownerId = null)
        {
            var owner = fake.an<Owner>();
            owner.WhenToldTo(x => x.OwnerId).Return(ownerId ?? new OwnerId());
            return owner;
        }
    }
}