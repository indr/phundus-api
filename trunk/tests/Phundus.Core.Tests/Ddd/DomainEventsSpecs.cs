namespace Phundus.Tests.Ddd
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Machine.Specifications;

    public class domain_events_concern
    {
        protected static IEnumerable<Type> domainEventTypes;

        public Establish c = () =>
        {
            domainEventTypes = Assembly.GetAssembly(typeof (CoreInstaller))
                .GetTypes().Where(p => p.IsSubclassOf(typeof (DomainEvent)));
        };
    }

    [Subject("DomainEvents")]
    public class all_subclasses_of_domain_event : domain_events_concern
    {
        public It should_have_data_contract_attribute = () => domainEventTypes.ShouldEachConformTo(
            t => t.GetCustomAttributes(typeof (DataContractAttribute), false).Length == 1);

        public It should_have_parameterless_constructor = () =>
            domainEventTypes.ShouldEachConformTo(
                t => t.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
                    new Type[] {}, null) != null);
    }

    [Subject("Domain event properties")]
    public class all_domain_event_properties : domain_events_concern
    {
        private static IList<PropertyInfo> properties = new List<PropertyInfo>();

        private Establish ctx = () =>
        {
            foreach (var each in domainEventTypes)
            {
                foreach (var prop in each.GetProperties())
                {
                    properties.Add(prop);
                }
            }
        };

        private It should_not_have_a_public_setter = () =>
        {
            var types = properties.Where(p => p.GetSetMethod() != null).Select(s => s.DeclaringType).Distinct();
            types.ShouldBeEmpty();
        };
    }
}