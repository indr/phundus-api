using System;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.TestHelpers.Builders
{
    public class UserBuilder : BuilderBase<User>
    {
        private bool _approved = true;
        private string _email = "user@example.com";
        private bool _loggedIn = true;
        private string _password = "1234";
        private Role _role;

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

        public UserBuilder Admin()
        {
            _role = new RoleBuilder().Admin();
            _email = "admin@example.com";
            return this;
        }

        public override User Build()
        {
            var result = new User();
            if (_role == null)
                _role = new RoleBuilder().User();
            result.Role = _role;
            result.Membership.Email = _email;
            result.Membership.IsApproved = _approved;
            result.Membership.Password = _password;

            if (_loggedIn)
                result.Membership.LogOn(Guid.NewGuid().ToString("N"), _password);
            Persist(result);
            return result;
        }


        public UserBuilder WithRole(Role role)
        {
            _role = role;
            return this;
        }
    }
}