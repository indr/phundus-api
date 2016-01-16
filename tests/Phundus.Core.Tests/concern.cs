namespace Phundus.Tests
{
    using System;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Common.Domain.Model;
    using developwithpassion.specifications.core;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Fakes;
    using Machine.Specifications;
    using Phundus.Ddd;
    using Phundus.Shop.Orders.Model;

   

    public class Factory 
    {
        private ICreateFakes fake;
        private Random _random;

        public Factory(ICreateFakes fake)
        {
            _random = new Random();
            this.fake = fake;
        }

        public Lessor Lessor()
        {
            var lessor = fake.an<Lessor>();
            lessor.WhenToldTo(x => x.LessorId).Return(new LessorId());
            return lessor;
        }

        public Article Article()
        {
            var article = fake.an<Article>();
            article.WhenToldTo(x => x.ArticleId).Return(new ArticleId(_random.Next(0, int.MaxValue)));
            return article;
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