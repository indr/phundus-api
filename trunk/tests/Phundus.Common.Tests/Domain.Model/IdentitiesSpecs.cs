namespace Phundus.Common.Tests.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using Common.Domain.Model;
    using Machine.Specifications;

    public class all_types_which_are_assignable_to_iidentity_guid
    {
        protected static List<Type> types;

        private Establish ctx = () =>
        {
            types = typeof (IIdentity<>).Assembly.GetTypes()
                .Where(p => typeof (IIdentity<Guid>).IsAssignableFrom(p))
                .Where(p => p != typeof (GuidIdentity))
                .ToList();
        };
    }

    [Subject("IIdentity<Guid>")]
    public class all_types : all_types_which_are_assignable_to_iidentity_guid
    {
        private It should_have_a_converter_specified = () =>
            types.ShouldEachConformTo(c =>
                c.IsDefined(typeof (TypeConverterAttribute), false));
    }

    [Subject("IIdentity<Guid>")]
    public class all_type_converter_attributes : all_types_which_are_assignable_to_iidentity_guid
    {
        private static Dictionary<Type, TypeConverterAttribute> attributes;
        private static string phundusCommonAssemblyFullName = typeof (GuidIdentity).Assembly.FullName;

        private Establish ctx = () =>
        {
            var attribute = typeof (TypeConverterAttribute);

            attributes = new Dictionary<Type, TypeConverterAttribute>();
            foreach (var each in types)
            {
                var typeConverterAttribute =
                    (TypeConverterAttribute)
                        each.GetCustomAttributes(attribute, false).SingleOrDefault(p => p.GetType() == attribute);
                if (typeConverterAttribute == null) continue;

                attributes.Add(each, typeConverterAttribute);
            }
        };

        private It should_be_of_guid_convert_T = () =>
            attributes.ShouldEachConformTo(c =>
                c.Value.ConverterTypeName == GetConverterType(c.Key));

        private static string GetConverterType(Type idType)
        {
            var assemblyQualifiedName = idType.AssemblyQualifiedName;
            var result = String.Format(
                @"Phundus.Common.Domain.Model.GuidConverter`1[[{0}]], {1}",
                assemblyQualifiedName, phundusCommonAssemblyFullName);
            return result;
        }
    }

    [Subject("IIdentity<Guid>")]
    public class when_instantiating_with_default_constructor : all_types_which_are_assignable_to_iidentity_guid
    {
        protected static ICollection<ConstructorInfo> constructors;
        protected static ICollection<IIdentity<Guid>> instances;

        private Establish ctx = () =>
            constructors = types.Select(s => s.GetConstructor(Type.EmptyTypes)).Where(p => p != null).ToList();

        private Because of = () =>
            instances = constructors.Select(s => (IIdentity<Guid>) s.Invoke(new object[0])).ToList();

        private It should_have_at_least_one_constructor = () =>
            constructors.Count().ShouldBeGreaterThan(0);

        private It should_have_at_least_one_instance = () =>
            instances.Count().ShouldBeGreaterThan(0);

        private It should_have_same_constructors_count_as_types_count = () =>
            constructors.Count.ShouldEqual(types.Count);

        private It should_not_have_empty_guid = () =>
            instances.ShouldEachConformTo(c => c.Id != Guid.Empty);
    }

    [Subject("IIdentity<Guid>")]
    public class when_instantiating_with_empty_guid : all_types_which_are_assignable_to_iidentity_guid
    {
        protected static IEnumerable<ConstructorInfo> constructors;

        private Establish ctx = () =>
            types = types.Where(p => p.GetConstructor(new[] {typeof (Guid)}) != null).ToList();

        private It should_throw_argument_null_exception = () =>
            types.ShouldEachConformTo(c => Catch.Exception(() =>
                c.GetConstructor(new[] {typeof (Guid)}).Invoke(new object[] {Guid.Empty})).InnerException.GetType() ==
                                           typeof (ArgumentNullException));
    }
}