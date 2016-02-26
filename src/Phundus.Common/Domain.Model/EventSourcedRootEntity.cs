namespace Phundus.Common.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public abstract class EventSourcedRootEntity : EntityWithCompositeId
    {
        private readonly List<IDomainEvent> _mutatingEvents;
        private readonly int _unmutatedVersion;

        protected EventSourcedRootEntity()
        {
            _mutatingEvents = new List<IDomainEvent>();
        }

        protected EventSourcedRootEntity(IEnumerable<IDomainEvent> eventStream, int streamVersion) : this()
        {
            foreach (var e in eventStream)
                When(e);

            _unmutatedVersion = streamVersion;
        }

        protected int UnmutatedVersion
        {
            get { return _unmutatedVersion; }
        }

        public IList<IDomainEvent> MutatingEvents
        {
            get { return _mutatingEvents; }
        }

        public int MutatedVersion
        {
            get { return _unmutatedVersion + 1; }
        }

        protected void Apply(IDomainEvent e)
        {
            _mutatingEvents.Add(e);
            When(e);
        }

        protected void Apply(IEnumerable<IDomainEvent> es)
        {
            foreach (var each in es)
                Apply(each);
        }

        protected void When(IDomainEvent e)
        {
            var whenMethod = FindWhenMethod(e.GetType());

            whenMethod.Invoke(this, new object[] {e});
        }

        private MethodInfo FindWhenMethod(Type type)
        {
            var candidates = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(p => p.Name == "When")
                .Where(p => p.GetParameters().Length == 1);

            var result = candidates.Where(p => p.GetParameters()[0].ParameterType == type).SingleOrDefault();

            if (result == null)
                throw new InvalidOperationException("Could not find When(" + type.Name + ")");

            return result;
        }
    }
}