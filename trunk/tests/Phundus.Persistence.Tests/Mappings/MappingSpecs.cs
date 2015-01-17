namespace Phundus.Persistence.Tests.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using developwithpassion.specifications.rhinomocks;
    using FluentNHibernate;
    using FluentNHibernate.Mapping;
    using FluentNHibernate.MappingModel.ClassBased;
    using Machine.Specifications;

    public class class_map_concern : Observes
    {
        private static IEnumerable<Type> classMapTypes;
        private static IEnumerable<IMappingProvider> instances;

        protected static IEnumerable<ClassMapping> _classMappings;
        protected static Func<Type, bool> _predicate;

        private Because of = () =>
        {
            classMapTypes = typeof (PersistenceInstaller).Assembly.GetTypes()
                .Where(p => IsSubclassOfOpenGeneric(typeof(ClassMap<>), p))
                .Where(_predicate);
            instances = classMapTypes.Select(each => (IMappingProvider) Activator.CreateInstance(each));
            _classMappings = instances.Select(each => each.GetClassMapping());
        };

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

    [Subject("ProjectionDataMappings")]
    public class all_projection_data_maps : class_map_concern
    {
        public Establish ctx = () =>
        {
            _predicate = p => p.Name.EndsWith("DataMap");
        };

        public It should_have_schema_action_all =
            () => _classMappings.ShouldEachConformTo(c => c.SchemaAction == "all");

        public It should_have_prefix_Proj_ =
            () => _classMappings.ShouldEachConformTo(c => c.TableName.StartsWith("Proj_",true, CultureInfo.InvariantCulture));
    }

    [Subject("NonProjectionDataMappings")]
    public class all_mappings_except_projection_data_maps : class_map_concern
    {
        public Establish ctx =
            () => { _predicate = p => !p.Name.EndsWith("DataMap"); };

        public It should_have_schema_action_validate =
            () => _classMappings.ShouldEachConformTo(c => c.SchemaAction == "validate");

        public It should_not_have_prefix_Proj_ =
            () => _classMappings.ShouldEachConformTo(c => !c.TableName.StartsWith("Proj_", true, CultureInfo.InvariantCulture));
    }
}