using NUnit.Framework;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Business.Security.Constraints;

namespace phiNdus.fundus.Business.UnitTests.Security.Constraints
{
    [TestFixture]
    public class AndConstraintTests
    {
        private static AndConstraint And(bool c1, bool c2)
        {
            return new AndConstraint(c1 ? (AbstractConstraint) new AlwaysTrueConstraint() : new AlwaysFalseConstraint(),
                                     c2 ? (AbstractConstraint) new AlwaysTrueConstraint() : new AlwaysFalseConstraint());
        }

        [Test]
        public void Eval_false_and_false_returns_false()
        {
            Assert.That(And(false, false).Eval(null), Is.False);
        }

        [Test]
        public void Eval_false_and_true_returns_false()
        {
            Assert.That(And(false, true).Eval(null), Is.False);
        }

        [Test]
        public void Eval_true_and_false_returns_false()
        {
            Assert.That(And(true, false).Eval(null), Is.False);
        }

        [Test]
        public void Eval_true_and_true_returns_true()
        {
            Assert.That(And(true, true).Eval(null), Is.True);
        }
    }
}