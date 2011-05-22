using System.Reflection;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Business.UnitTests.Security
{
    internal static class SessionHelper
    {
        public static SecuritySession CreateSession(User user, string key)
        {
            var info = typeof (SecuritySession).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic
                , null, new[] {typeof (User), typeof (string)}, null);
            return (SecuritySession) info.Invoke(new object[] {user, key});
        }
    }
}