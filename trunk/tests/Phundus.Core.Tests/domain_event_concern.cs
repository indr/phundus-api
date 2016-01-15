namespace Phundus.Tests
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using Castle.Core.Internal;
    using Common.Domain.Model;
    using Machine.Specifications;

    public class domain_event_concern<T>
    {
        protected static Type type = typeof (T);
        protected static T sut;
        protected static UserGuid theInitiatorGuid;

        private Establish ctx = () => theInitiatorGuid = new UserGuid();

        protected static object dataMember(int order)
        {
            var dataMemberProperties = type.GetProperties().Where(p => AttributesUtil.HasAttribute<DataMemberAttribute>(p)).ToList();
            foreach (var propertyInfo in dataMemberProperties)
            {
                var attribute = (DataMemberAttribute)propertyInfo.GetCustomAttributes(
                    typeof (DataMemberAttribute), false).Single();
                if (attribute.Order == order)
                    return propertyInfo.GetValue(sut, null);
            }
            throw new Exception(String.Format("Could not find property with data member order {0}.", order));
        }
    }
}