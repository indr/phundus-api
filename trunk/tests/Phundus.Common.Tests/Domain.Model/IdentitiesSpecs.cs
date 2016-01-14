namespace Phundus.Common.Tests.Domain.Model
{
    using System;
    using System.Collections.Generic;
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
                .ToList();
        };
    }

    [Subject(typeof (IIdentity<Guid>))]
    public class when_instantiating_with_default_constructor : all_types_which_are_assignable_to_iidentity_guid
    {
        protected static IEnumerable<ConstructorInfo> constructors;
        protected static IEnumerable<IIdentity<Guid>> instances;

        private Establish ctx = () => constructors = types.Select(s => s.GetConstructor(Type.EmptyTypes)).Where(p => p != null);

        private Because of = () => instances = constructors.Select(s => (IIdentity<Guid>) s.Invoke(new object[0]));

        private It should_not_have_empty_guid = () => instances.ShouldEachConformTo(c => c.Id != Guid.Empty);
    }

    [Subject(typeof (IIdentity<Guid>))]
    public class when_instantiating_with_empty_guid : all_types_which_are_assignable_to_iidentity_guid
    {
        protected static IEnumerable<ConstructorInfo> constructors;
      
        private Establish ctx = () => types = types.Where(p => p.GetConstructor(new Type[] {typeof(Guid)}) != null).ToList();

        private It should_throw_argument_exception = () => types.ShouldEachConformTo(c => Catch.Exception(
            () => c.GetConstructor(new Type[] {typeof (Guid)}).Invoke(new object[] {Guid.Empty})) != null);

    }
}