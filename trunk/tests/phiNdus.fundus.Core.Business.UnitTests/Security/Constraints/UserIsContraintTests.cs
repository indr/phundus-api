using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Security.Constraints;
using User = phiNdus.fundus.Core.Domain.Entities.User;

namespace phiNdus.fundus.Core.Business.UnitTests.Security.Constraints
{
    [TestFixture]
    public class UserIsContraintTests : BaseConstraintTestFixture
    {
        [Test]
        public void Eval_with_SessionUsers_id_does_not_equal_returns_false()
        {
            var user = new User(1);
            AbstractConstraint sut = Business.Security.Constraints.User.Is(2);
            Assert.That(sut.Eval(SecurityContext(user, null)), Is.False);
        }

        [Test]
        public void Eval_with_SessionUsers_id_equals_returns_true()
        {
            var user = new User(1);
            AbstractConstraint sut = Business.Security.Constraints.User.Is(1);
            Assert.That(sut.Eval(SecurityContext(user, null)), Is.True);
        }
    }
}
