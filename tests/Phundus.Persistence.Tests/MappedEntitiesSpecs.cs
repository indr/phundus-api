namespace Phundus.Persistence.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FluentNHibernate.Mapping;
    using Machine.Specifications;

    public class class_maps
    {
        protected static List<Type> classMaps;

        protected static List<Type> mappedTypes;

        private Establish ctx = () =>
        {
            classMaps = typeof (NHibernateInstaller).Assembly.GetTypes()
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType &&
                            t.BaseType.GetGenericTypeDefinition() == typeof (ClassMap<>))
                .ToList();

            mappedTypes = classMaps.Select(s => s.BaseType.GetGenericArguments()[0]).ToList();
        };
    }

    [Subject("Mapped entities")]
    public class public_or_protected_methods_of_mapped_entites : class_maps
    {
        private static List<MethodInfo> methods;

        private Because of = () =>
            methods =
                mappedTypes.SelectMany(
                    s => s.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    .Where(p => p.DeclaringType != typeof (Object))
                    .Where(p => !p.IsConstructor)
                    .Where(p => !p.IsPrivate)
                    .ToList();

        private It should_be_virtual = () =>
            methods.ShouldEachConformTo(c => c.IsVirtual);

        private It should_find_method_infos = () =>
            methods.ShouldNotBeEmpty();
    }

    [Subject("Mapped entities")]
    public class public_or_protected_properties_of_mapped_entites : class_maps
    {
        private Because of = () =>
        {
            propertyInfos = mappedTypes.SelectMany(s => s.GetProperties()).ToList();
        };

        private static List<PropertyInfo> propertyInfos;

        private It should_be_virtual = () =>
            propertyInfos.ShouldEachConformTo(p => (p.CanRead ? p.GetGetMethod(true) : p.GetSetMethod(true)).IsVirtual);

    }
}