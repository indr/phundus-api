using System.Reflection;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.TestHelpers.Builders
{
    public class SecuritySessionBuilder : BuilderBase<SecuritySession>
    {
        private readonly string _key;
        private readonly User _user;

        public SecuritySessionBuilder(User user, string key)
        {
            _user = user;
            _key = key;
        }

        public override SecuritySession Build()
        {
            var info = typeof(SecuritySession).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic
                , null, new[] { typeof(User), typeof(string) }, null);
            return (SecuritySession)info.Invoke(new object[] { _user, _key });
        }
    }
}