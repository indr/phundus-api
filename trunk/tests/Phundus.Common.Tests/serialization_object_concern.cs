namespace Phundus.Common.Tests
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    public class serialization_object_concern<T> : Observes<T> where T : class
    {
        protected static Type type = typeof(T);
        protected static string itsFullName;
        protected static string itsAssembly;

        private Establish ctx = () =>
        {
            itsFullName = type.FullName;
            itsAssembly = type.Assembly.GetName().Name;
        };

        protected static object dataMember(int order)
        {
            var dataMemberProperties = type.GetProperties().Where(
                p => p.GetCustomAttributes(typeof(DataMemberAttribute), false).Length == 1).ToList();
            foreach (var propertyInfo in dataMemberProperties)
            {
                var attribute = (DataMemberAttribute)propertyInfo.GetCustomAttributes(
                    typeof(DataMemberAttribute), false).Single();
                if (attribute.Order == order)
                    return propertyInfo.GetValue(sut, null);
            }
            throw new Exception(String.Format("Could not find property with data member order {0}.", order));
        }
    }
}