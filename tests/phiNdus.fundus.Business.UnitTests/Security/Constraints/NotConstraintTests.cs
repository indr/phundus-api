using NUnit.Framework;
using phiNdus.fundus.Business.Security.Constraints;

namespace phiNdus.fundus.Business.UnitTests.Security.Constraints
{
    [TestFixture]
    public class NotConstraintTests
    {
        [Test]
        public void Eval_false_is_true()
        {
            Assert.That(new NotConstraint(new AlwaysFalseConstraint()).Eval(null), Is.True);
        }

        [Test]
        public void Eval_true_is_false()
        {
            Assert.That(new NotConstraint(new AlwaysTrueConstraint()).Eval(null), Is.False);
        }
    }
}