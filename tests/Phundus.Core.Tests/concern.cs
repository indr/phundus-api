namespace Phundus.Core.Tests
{
    using System;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Ddd;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public abstract class concern<TClass> : Observes<TClass> where TClass : class
    {
        // ReSharper disable StaticFieldInGenericType

        private static IWindsorContainer container;

        protected static Mock mock = new Mock();

        protected static IEventPublisher publisher;

        // ReSharper restore StaticFieldInGenericType

        protected Establish event_publisher = () =>
        {
            publisher = depends.@on<IEventPublisher>();

            container = new WindsorContainer();
            container.Register(Component.For<IEventPublisher>().Instance(publisher));
            EventPublisher.Container = container;
        };
    }
}