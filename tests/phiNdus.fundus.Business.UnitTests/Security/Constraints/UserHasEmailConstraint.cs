using NUnit.Framework;
using phiNdus.fundus.Business.Security.Constraints;
using User = phiNdus.fundus.Domain.Entities.User;

namespace phiNdus.fundus.Business.UnitTests.Security.Constraints
{
    [TestFixture]
    public class UserHasEmailConstraint : BaseConstraintTestFixture
    {
        [Test]
        public void Eval_with_SessionUsers_email_does_not_equal_returns_false()
        {
            var user = new User();
            user.Membership.Email = "user1@example.com";
            AbstractConstraint sut = Business.Security.Constraints.User.HasEmail("user2@example.com");
            Assert.That(sut.Eval(SecurityContext(user, null)), Is.False);
        }

        [Test]
        public void Eval_with_SessionUsers_email_equals_returns_true()
        {
            var user = new User();
            user.Membership.Email = "user@example.com";
            AbstractConstraint sut = Business.Security.Constraints.User.HasEmail("user@example.com");
            Assert.That(sut.Eval(SecurityContext(user, null)), Is.True);
        }
    }
}