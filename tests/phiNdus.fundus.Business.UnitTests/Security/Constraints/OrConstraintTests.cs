using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Security.Constraints;

namespace phiNdus.fundus.Core.Business.UnitTests.Security.Constraints
{
    [TestFixture]
    public class OrConstraintTests
    {
        private static OrConstraint Or(bool c1, bool c2)
        {
            return new OrConstraint(c1 ? (AbstractConstraint)new AlwaysTrueConstraint() : new AlwaysFalseConstraint(),
                                    c2 ? (AbstractConstraint)new AlwaysTrueConstraint() : new AlwaysFalseConstraint());
        }

        [Test]
        public void Eval_false_and_false_returns_false()
        {
            Assert.That(Or(false, false).Eval(null), Is.False);
        }

        [Test]
        public void Eval_false_and_true_returns_true()
        {
            Assert.That(Or(false, true).Eval(null), Is.True);
        }

        [Test]
        public void Eval_true_and_false_returns_true()
        {
            Assert.That(Or(true, false).Eval(null), Is.True);
        }

        [Test]
        public void Eval_true_and_true_returns_true()
        {
            Assert.That(Or(true, true).Eval(null), Is.True);
        }
    }
}
