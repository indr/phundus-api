namespace Phundus.Tests.Commanding
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Common.Commanding;
    using Machine.Specifications;

    [Subject("Command handler properties")]
    public class every_command_should
    {
        private static Dictionary<PropertyInfo, string> propertiesWithoutProtectedSetter;

        private Establish ctx = () =>
        {
            var commands = Assembly.GetAssembly(typeof (CoreInstaller)).GetTypes()
                .Where(p => !p.IsAbstract)
                .Where(p => typeof (ICommand).IsAssignableFrom(p));

            propertiesWithoutProtectedSetter = commands
                .SelectMany(s => s.GetProperties(BindingFlags.SetProperty | BindingFlags.Instance
                                                 | BindingFlags.Public | BindingFlags.NonPublic))
                .Where(p => !HasProtectedSetter(p))
                .ToDictionary(p => p, p => p.DeclaringType.Name);
        };

        private It should_have_only_protected_setter = () =>
            propertiesWithoutProtectedSetter.ShouldBeEmpty();

        private static bool HasProtectedSetter(PropertyInfo arg)
        {
            if (!arg.CanWrite)
                return false;
            var info = arg.GetSetMethod(true);
            if (info == null)
                return false;

            return !info.IsPrivate && !info.IsPublic;
        }
    }
}