using System;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.TestHelpers.Builders
{
    public class UserBuilder : BuilderBase<User>
    {
        private string _email = "anonymous@example.com";

        protected override void Persist(User obj)
        {
            if (UnitOfWork.IsStarted)
            {
                UnitOfWork.CurrentSession.Delete(String.Format("from Membership m where m.Email = '{0}'",
                                                               obj.Membership.Email));
                UnitOfWork.CurrentSession.Flush();
            }

            base.Persist(obj);
        }

        public UserBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public override User Build()
        {
            var result = new User();
            result.Role = new RoleBuilder().User();
            result.Membership.Email = _email;
            Persist(result);
            return result;
        }
    }
}