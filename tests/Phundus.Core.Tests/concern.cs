namespace Phundus.Tests
{
    using System;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Common.Domain.Model;
    using developwithpassion.specifications.core;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Ddd;
    using Phundus.Shop.Orders.Model;

    public class Factory
    {
        private Random _random;
        private ICreateFakes fake;

        public Factory(ICreateFakes fake)
        {
            _random = new Random();
            this.fake = fake;
        }

        public Article Article()
        {
            var article = fake.an<Article>();
            article.setup(x => x.ArticleId).Return(new ArticleId(NextNumericId()));
            return article;
        }

        public Lessor Lessor()
        {
            var lessor = fake.an<Lessor>();
            lessor.setup(x => x.LessorId).Return(new LessorId());
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

        private int NextNumericId()
        {
            return _random.Next(0, int.MaxValue);
        }
    }

    public abstract class concern<TClass> : Observes<TClass> where TClass : class
    {
        // ReSharper disable StaticFieldInGenericType

        private static IWindsorContainer container;

        protected static Mock mock = new Mock();

        protected static IEventPublisher publisher;

        protected static Factory make;

        // ReSharper restore StaticFieldInGenericType

        private Establish ctx = () =>
        {
            make = new Factory(fake);
            publisher = depends.@on<IEventPublisher>();

            container = new WindsorContainer();
            container.Register(Component.For<IEventPublisher>().Instance(publisher));
            EventPublisher.Container = container;
        };
    }
}