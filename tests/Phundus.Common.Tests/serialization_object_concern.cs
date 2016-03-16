namespace Phundus.Common.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using ProtoBuf;

    public class serialization_object_concern<T> : Observes<T> where T : class
    {
        protected static Type type = typeof (T);
        protected static string itsFullName;
        protected static string itsAssembly;

        private Establish ctx = () =>
        {
            itsFullName = type.FullName;
            itsAssembly = type.Assembly.GetName().Name;
        };

        protected Because of = () =>
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, sut);
            stream.Seek(0, SeekOrigin.Begin);
            sut = Serializer.Deserialize<T>(stream);
            sut.ShouldNotBeNull();
        };

        protected static object dataMember(int order)
        {
            var dataMemberProperties =
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(
                    p => p.GetCustomAttributes(typeof (DataMemberAttribute), false).Length == 1).ToList();
            foreach (var propertyInfo in dataMemberProperties)
            {
                var attribute = (DataMemberAttribute) propertyInfo.GetCustomAttributes(
                    typeof (DataMemberAttribute), true).Single();
                if (attribute.Order == order)
                    return propertyInfo.GetValue(sut, null);
            }
            throw new Exception(String.Format("Could not find property with data member order {0}.", order));
        }

        protected static bool noDataMember(int order)
        {
            try
            {
                dataMember(order);
                return false;
            }
            catch (Exception ex)
            {
                return ex.Message == String.Format("Could not find property with data member order {0}.", order);
            }
        }
    }
}