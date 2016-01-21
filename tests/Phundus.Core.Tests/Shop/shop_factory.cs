namespace Phundus.Tests.Shop
{
    using Common.Domain.Model;
    using developwithpassion.specifications.core;
    using developwithpassion.specifications.extensions;
    using Phundus.Shop.Orders.Model;

    public class shop_factory : factory_base
    {
        public shop_factory(ICreateFakes fake) : base(fake)
        {
        }

        public Article ShopArticle()
        {
            var article = fake.an<Article>();
            article.setup(x => x.ArticleId).Return(new ArticleId(NextNumericId()));
            article.setup(x => x.LessorId).Return(new LessorId());
            return article;
        }

        public Lessor Lessor(LessorId lessorId = null)
        {
            var lessor = fake.an<Lessor>();
            lessor.setup(x => x.LessorId).Return(lessorId ?? new LessorId());
            return lessor;
        }

        public Order Order()
        {
            var order = fake.an<Order>();
            var orderId = new OrderId(NextNumericId());
            order.setup(x => x.Id).Return(orderId.Id);
            order.setup(x => x.OrderId).Return(orderId);
            return order;
        }
    }
}