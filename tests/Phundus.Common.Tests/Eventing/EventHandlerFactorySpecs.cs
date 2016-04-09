namespace Phundus.Common.Tests.Eventing
{
    using System.Reflection;
    using Castle.DynamicProxy;
    using Castle.MicroKernel;
    using Common.Domain.Model;
    using Common.Eventing;
    using Common.Eventing.Installers;
    using Machine.Specifications;

    public class event_handler_factory_concern : container_concern<IEventHandlerFactory>
    {
        private Establish ctx = () =>
        {
            container.Install(new EventingInstaller());
            new EventHandlerInstaller().Install(container, Assembly.GetExecutingAssembly());
            sut_factory.create_using(resolve<IEventHandlerFactory>);
        };

        public class TestDomainEvent1 : DomainEvent
        {
        }

        public class TestDomainEvent2 : DomainEvent
        {
        }

        public class TestSubscriber1 : ISubscribeTo<TestDomainEvent1>
        {
            public void Handle(TestDomainEvent1 e)
            {
                throw new System.NotImplementedException();
            }
        }

        public class TestSubscriber2 : ISubscribeTo<TestDomainEvent1>, ISubscribeTo<TestDomainEvent2>
        {
            public void Handle(TestDomainEvent1 e)
            {
                throw new System.NotImplementedException();
            }

            public void Handle(TestDomainEvent2 e)
            {
                throw new System.NotImplementedException();
            }
        }
    }

    public class when_get_subscriber_with_unknown_full_name : event_handler_factory_concern
    {
        private static ISubscribeTo subscriber;

        private Because of = () =>
            spec.catch_exception(() =>
                subscriber = sut.GetSubscriber("Unknown type"));

        private It should_not_return_subscriber = () =>
            subscriber.ShouldBeNull();

        private It should_throw_component_not_found_exception = () =>
            spec.exception_thrown.ShouldBeOfExactType<ComponentNotFoundException>();
    }

    public class when_get_subscriber_1_with_known_full_name : event_handler_factory_concern
    {
        // Note: Default component resolver resolves the first element.

        private static ISubscribeTo subscriber;

        private Because of = () =>
            subscriber = sut.GetSubscriber(typeof (TestSubscriber1).FullName);

        private It should_return_subscriber = () =>
            ProxyUtil.GetUnproxiedType(subscriber).ShouldEqual(typeof (TestSubscriber1));
    }

    public class when_get_subscriber_2_with_known_full_name : event_handler_factory_concern
    {
        // Note: Default component resolver resolves the first element.

        private static ISubscribeTo subscriber;

        private Because of = () =>
            subscriber = sut.GetSubscriber(typeof (TestSubscriber2).FullName);

        private It should_return_subscriber = () =>
            ProxyUtil.GetUnproxiedType(subscriber).ShouldEqual(typeof (TestSubscriber2));
    }

    public class when_get_subscribers : event_handler_factory_concern
    {
        private static ISubscribeTo[] subscribers;

        private Because of = () =>
            subscribers = sut.GetSubscribers();

        private It should_return_array_that_contains_test_subscriber_1 = () =>
            subscribers.ShouldContain(p => ProxyUtil.GetUnproxiedType(p) == typeof (TestSubscriber1));

        private It should_return_array_that_contains_test_subscriber_2 = () =>
            subscribers.ShouldContain(p => ProxyUtil.GetUnproxiedType(p) == typeof (TestSubscriber2));
    }

    public class when_get_subscribers_for_event_generic : event_handler_factory_concern
    {
        private static ISubscribeTo<TestDomainEvent2>[] subscribers;

        private Because of = () =>
            subscribers = sut.GetSubscribersForEvent(new TestDomainEvent2());

        private It should_return_array_that_contains_test_subscriber_2 = () =>
            subscribers.ShouldContain(p => ProxyUtil.GetUnproxiedType(p) == typeof (TestSubscriber2));
    }

    public class when_get_subscribers_for_event_non_generic : event_handler_factory_concern
    {
        private static ISubscribeTo[] subscribers;

        private Because of = () =>
            subscribers = sut.GetSubscribersForEventNonGeneric(new TestDomainEvent2());

        private It should_return_array_that_contains_only_test_subscriber_2 = () =>
        {
            subscribers.ShouldContain(p => ProxyUtil.GetUnproxiedType(p) == typeof (TestSubscriber2));
            subscribers.Length.ShouldEqual(1);
        };
    }
}