namespace Phundus.Common.Tests.Domain.Model
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;

    [Subject(typeof (Initiator))]
    public class when_instantiating_an_initiator : Observes<Initiator>
    {
        private static InitiatorId theInitiatorId;
        private static string theEmailAddress;
        private static string theFullName;

        private static Type type = typeof (Initiator);

        private Establish ctx = () =>
        {
            theInitiatorId = new InitiatorId();
            theEmailAddress = "initiator@test.phundus.ch";
            theFullName = "The Initiator";
            sut_factory.create_using(
                () => new Initiator(theInitiatorId, theEmailAddress, theFullName));
        };

        private It should_be_in_assembly = () =>
            type.Assembly.GetName().Name.ShouldEqual("Phundus.Common");

        private It should_be_in_namespace = () =>
            type.Namespace.ShouldEqual("Phundus.Common.Domain.Model");

        private It should_have_the_initiator_id_at_1 = () =>
            dataMember(1).ShouldEqual(theInitiatorId.Id);

        private It should_have_the_email_address_at_2 = () =>
            dataMember(2).ShouldEqual(theEmailAddress);

        private It should_have_the_full_name_at_3 = () =>
            dataMember(3).ShouldEqual(theFullName);

        // TODO: Code duplication, see domain_event_concern in Phundus.Core.Tests
        protected static object dataMember(int order)
        {
            var dataMemberProperties = type.GetProperties().Where(
                p => p.GetCustomAttributes(typeof (DataMemberAttribute), false).Length == 1).ToList();
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