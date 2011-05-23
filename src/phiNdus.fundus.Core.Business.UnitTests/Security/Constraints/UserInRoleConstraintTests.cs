using NUnit.Framework;
using phiNdus.fundus.Core.Business.Security.Constraints;
using phiNdus.fundus.Core.Domain.Entities;
using User = phiNdus.fundus.Core.Domain.Entities.User;

namespace phiNdus.fundus.Core.Business.UnitTests.Security.Constraints
{
    [TestFixture]
    internal class UserInRoleConstraintTests : BaseConstraintTestFixture
    {
        #region SetUp/TearDown

        [SetUp]
        public void SetUp()
        {
            User = new User();
            User.Role = Role.User;
            Administrator = new User();
            Administrator.Role = Role.Administrator;
        }

        #endregion

        protected AbstractConstraint Sut { get; set; }
        protected User User { get; set; }
        protected User Administrator { get; set; }

        [Test]
        public void Eval_with_user_in_role_returns_true()
        {
            Sut = Business.Security.Constraints.User.InRole(Role.Administrator);
            Assert.That(Sut.Eval(SecurityContext(Administrator, "")), Is.True);
        }

        [Test]
        public void Eval_with_user_not_in_role_returns_false()
        {
            Sut = Business.Security.Constraints.User.InRole(Role.Administrator);
            Assert.That(Sut.Eval(SecurityContext(User, "")), Is.False);
        }
    }
}