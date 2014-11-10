namespace Phundus.Core.Tests.Ddd
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Machine.Specifications;

    [Subject("DomainEvents")]
    public class all_subclasses_of_domain_event
    {
        private static IEnumerable<Type> domainEventTypes;
        private static IEnumerable<ConstructorInfo> constructorInfos;

        public Establish c = () =>
        {
            domainEventTypes =
                Assembly.GetAssembly(typeof (CoreInstaller)).GetTypes().Where(p => p.IsSubclassOf(typeof (DomainEvent)));

            constructorInfos = domainEventTypes.Select(
                s => s.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
                    new Type[] {}, null));
        };

        public It should_have_DataContract_attribute =
            () => domainEventTypes.ShouldEachConformTo(
                t => t.GetCustomAttributes(typeof (DataContractAttribute), false).Length == 1);

        public It should_have_parameterless_constructor =
            () =>
                domainEventTypes.ShouldEachConformTo(
                    t => t.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
                        new Type[] {}, null) != null);

        public It should_have_working_parameterless_constructor =
            () => constructorInfos.ShouldEachConformTo(c => c.Invoke(new object[] {}) != null);
    }
}