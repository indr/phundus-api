namespace Phundus.Tests
{
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Phundus.Ddd;

    public abstract class concern<TClass> : Observes<TClass> where TClass : class
    {
        // ReSharper disable StaticFieldInGenericType


        protected static Mock mock = new Mock();

        protected static IEventPublisher publisher;


        // ReSharper restore StaticFieldInGenericType

        private Cleanup cleanup = () => EventPublisher.Factory(null);

        private Establish ctx = () =>
        {
            publisher = depends.@on<IEventPublisher>();
            EventPublisher.Factory(() => publisher);
        };
    }
}