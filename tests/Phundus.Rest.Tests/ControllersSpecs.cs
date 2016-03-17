namespace Phundus.Rest.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Api;
    using Api.Organizations;
    using AttributeRouting.Web.Http;
    using Castle.Transactions;
    using Common.Resources;
    using Machine.Specifications;

    [Subject("Controllers transaction attribute")]
    public class controllers_non_get_methods
    {
        private static IEnumerable<KeyValuePair<MethodInfo, Type>> methodsWithTransactionAttribute;

        private static IList<object> HttpAttributes = new List<object>
        {
            typeof (DELETEAttribute),
            typeof (POSTAttribute),
            typeof (PATCHAttribute),
            typeof (PUTAttribute)
        };

        private static IList<object> exclusions = new List<object>
        {
            typeof(OrganizationsFilesController),
            typeof(SessionsController)
        }; 

        private Establish ctx = () =>
        {
            var controllerTypes = Assembly.GetAssembly(typeof (Installer)).GetTypes()
                .Where(p => p.BaseType == typeof (ApiControllerBase))
                .Where(p => !p.IsAbstract)
                .Where(p => !exclusions.Contains(p))
                .ToList();


            var methods = controllerTypes
                .SelectMany(p => p.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                .Where(HasHttpMethodAttributeButGet)
                .ToDictionary(m => m, m => m.DeclaringType);

            methodsWithTransactionAttribute = methods.Where(kvp => HasTransactionAttribute(kvp.Key));
        };


        private It should_not_have_a_transaction_attribute = () =>
            methodsWithTransactionAttribute.ShouldBeEmpty();

        private static bool HasHttpMethodAttributeButGet(MethodInfo arg)
        {
            var attributes = arg.GetCustomAttributes(true).Select(s => s.GetType()).ToList();
            return attributes.Intersect(HttpAttributes).Any();
        }

        private static bool HasTransactionAttribute(MethodInfo arg)
        {
            return arg.GetCustomAttributes(typeof (TransactionAttribute), true).Any();
        }
    }
}