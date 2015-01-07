namespace Phundus.Core.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Common.Domain.Model;
    using Core.Ddd;
    using Core.Inventory.Domain.Model.Management;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public abstract class aggregate_root_concern<TAggregateRoot> where TAggregateRoot : EventSourcedRootEntity
    {
        private static IWindsorContainer container;

        protected static IEventPublisher publisher;

        protected static Mock mock;

        protected Establish event_publisher = () =>
        {
            _sut = default(TAggregateRoot);

            mock = new Mock();
            
            publisher = mock.stub<IEventPublisher>();

            container = new WindsorContainer();
            container.Register(Component.For<IEventPublisher>().Instance(publisher));
            EventPublisher.Container = container;
        };

        protected static TAggregateRoot _sut { get; set; }

        protected static TDomainEvent AssertMutatingEvent<TDomainEvent>(int idx)
        {
            aggregate_root_concern<Stock>._sut.MutatingEvents.Count.ShouldBeGreaterThan(idx);
            var evnt = aggregate_root_concern<Stock>._sut.MutatingEvents[0];
            evnt.ShouldBeOfExactType<TDomainEvent>();
            return (TDomainEvent) evnt;
        }
    }

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