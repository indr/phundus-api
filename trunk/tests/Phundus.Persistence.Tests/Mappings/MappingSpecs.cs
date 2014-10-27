namespace Phundus.Persistence.Tests.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using developwithpassion.specifications.rhinomocks;
    using FluentNHibernate;
    using FluentNHibernate.Mapping;
    using Machine.Specifications;

    [Subject("NHibernateMappings")]
    public class all_mappings_except_those_for_records : Observes
    {
        private static IEnumerable<Type> classMapTypes;
        private static IEnumerable<IMappingProvider> instances;

        public Establish ctx =
            () =>
            {
                classMapTypes = typeof (PersistenceInstaller).Assembly.GetTypes()
                    .Where(p => !p.Name.EndsWith("RecordMap"))
                    .Where(p => IsSubclassOfOpenGeneric(typeof (ClassMap<>), p));
            };

        public Because of =
            () => { instances = classMapTypes.Select(each => (IMappingProvider) Activator.CreateInstance(each)); };

        public It should_have_schema_action_validate =
            () => instances.ShouldEachConformTo(c => c.GetClassMapping().SchemaAction == "validate");

        private static bool IsSubclassOfOpenGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof (object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}