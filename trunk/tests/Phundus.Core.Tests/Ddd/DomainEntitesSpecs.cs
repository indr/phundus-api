namespace Phundus.Core.Tests.Ddd
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Core.Ddd;
    using Machine.Specifications;

    [Subject("DomainEntities")]
    public class all_subclasses_of_entity_base
    {
        private static IEnumerable<Type> entityBaseTypes;
        private static IEnumerable<MethodInfo> allPublicMethods;

        private Establish ctx = () =>
        {
            entityBaseTypes =
                Assembly.GetAssembly(typeof (CoreInstaller)).GetTypes().Where(p => p.IsSubclassOf(typeof (EntityBase)));
            allPublicMethods =
                entityBaseTypes.SelectMany(s => s.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(p => p.Name != "GetType").ToList());
        };

        public It should_have_parameterless_constructor =
            () =>
                entityBaseTypes.ShouldEachConformTo(
                    t => t.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null,
                        new Type[] {}, null) != null);

        public It should_have_public_methods_virtual =
            () =>
                allPublicMethods.ShouldEachConformTo(c => c.IsVirtual);
    }
}