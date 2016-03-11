namespace Phundus.Common.Tests.Mailing
{
    using System.Collections.Generic;
    using Machine.Specifications;

    public static class ShouldExtensions
    {
        public static void ShouldContainMember(this object obj, string memberName)
        {
            var members = (IDictionary<string, object>) obj;
            members.ShouldContain(p => p.Key == memberName);            
        }

        public static void ShouldContainMemberOfExactType<T>(this object obj, string memberName)
        {
            var members = (IDictionary<string, object>) obj;
            members.ShouldContain(p => p.Key == memberName);
            var member = members[memberName];
            member.ShouldBeOfExactType<T>();
        }
    }
}