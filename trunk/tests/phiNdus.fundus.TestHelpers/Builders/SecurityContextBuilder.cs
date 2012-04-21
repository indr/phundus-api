using System;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.TestHelpers.Builders
{
    public class SecurityContextBuilder : BuilderBase<SecurityContext>
    {
        private User _user;
        private string _key = Guid.NewGuid().ToString("N");

        public SecurityContextBuilder ForUser(User user)
        {
            _user = user;
            return this;
        }

        public SecurityContextBuilder WithKey(string key)
        {
            _key = key;
            return this;
        }

        public override SecurityContext Build()
        {
            var result = new SecurityContext();
            result.SecuritySession = new SecuritySessionBuilder(_user, _key).Build();
            return result;
        }
    }
}