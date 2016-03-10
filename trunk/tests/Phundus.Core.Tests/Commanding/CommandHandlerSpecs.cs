namespace Phundus.Tests.Commanding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Castle.Transactions;
    using Common;
    using Common.Commanding;
    using Machine.Specifications;

    [Subject("Command handlers transaction attribute")]
    public class every_command_handlers_handle_method
    {
        private static Dictionary<MethodInfo, Type> methodsWithoutTransactionAttribute;

        private Establish ctx = () =>
        {
            var handlers = GetTypes()
                .Where(p => !p.IsAbstract)
                .Where(p => typeof (IHandleCommand).IsAssignableFrom(p));

            var methods = handlers.Select(p => p.GetMethod("Handle"))
                .ToDictionary(m => m, m => m.DeclaringType);

            methodsWithoutTransactionAttribute = methods.Where(HasNoTransactionAttribute).ToDictionary(ks => ks.Key, ks => ks.Value);
        };

        private static IEnumerable<Type> GetTypes()
        {
            var result = new List<Type>();
            result.AddRange(Assembly.GetAssembly(typeof(CoreInstaller)).GetTypes());
            //result.AddRange(Assembly.GetAssembly(typeof(CommonInstaller)).GetTypes());
            return result;
        }

        private static bool HasNoTransactionAttribute(KeyValuePair<MethodInfo, Type> arg)
        {
            return !arg.Key.GetCustomAttributes(typeof (TransactionAttribute), true).Any();
        }

        private It should_have_transaction_attribute = () =>
            methodsWithoutTransactionAttribute.ShouldBeEmpty();
    }
}