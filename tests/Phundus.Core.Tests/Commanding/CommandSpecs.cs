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
        private static Dictionary<PropertyInfo, string> propertiesWithPublicSetter;

        private Establish ctx = () =>
        {
            var commands = Assembly.GetAssembly(typeof (CoreInstaller)).GetTypes()
                .Where(p => !p.IsAbstract)
                .Where(p => typeof (ICommand).IsAssignableFrom(p));

            propertiesWithPublicSetter = commands
                .SelectMany(s => s.GetProperties(BindingFlags.SetProperty | BindingFlags.Instance
                                                 | BindingFlags.Public | BindingFlags.NonPublic))
                .Where(p => HasPublicOrPrivateSetter(p))
                .ToDictionary(p => p, p => p.DeclaringType.Name);
        };

        private It should_have_only_protected_setter = () =>
            propertiesWithPublicSetter.ShouldBeEmpty();

        private static bool HasPublicOrPrivateSetter(PropertyInfo arg)
        {
            if (!arg.CanWrite)
                return false;
            var info = arg.GetSetMethod(true);
            if (info == null)
                return false;

            return info.IsPrivate || info.IsPublic;
        }
    }
}