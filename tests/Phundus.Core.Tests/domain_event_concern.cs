namespace Phundus.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Castle.Core.Internal;
    using Common.Domain.Model;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class domain_event_concern<T> : Observes<T> where T : class
    {
        protected static Type type = typeof (T);
        protected static InitiatorId theInitiatorId;
        protected static Initiator theInitiator;

        protected static string itsNamespace;
        protected static string itsAssembly;

        private Establish ctx = () =>
        {
            theInitiatorId = new InitiatorId();
            theInitiator = new Initiator(theInitiatorId, "initiator@test.phundus.ch", "The Initiator");
            itsNamespace = type.Namespace;
            itsAssembly = type.Assembly.GetName().Name;
        };

        protected static object dataMember(int order)
        {
            var dataMemberProperties = type.GetProperties().Where(p => p.HasAttribute<DataMemberAttribute>()).ToList();
            foreach (var propertyInfo in dataMemberProperties)
            {
                var attribute = (DataMemberAttribute) propertyInfo.GetCustomAttributes(
                    typeof (DataMemberAttribute), false).Single();
                if (attribute.Order == order)
                    return propertyInfo.GetValue(sut, null);
            }
            throw new Exception(String.Format("Could not find property with data member order {0}.", order));
        }
    }
}