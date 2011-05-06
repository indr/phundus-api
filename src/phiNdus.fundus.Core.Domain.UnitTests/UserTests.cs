using NUnit.Framework;

namespace phiNdus.fundus.Core.Domain.UnitTests
{
    [TestFixture]
    class UserTests
    {
        [Test]
        public void Can_create()
        {
            var user = new User();

            Assert.That(user.Membership, Is.Not.Null);
        }

        [Test]
        public void Get_and_set_FirstName()
        {
            var user = new User();
            Assert.That(user.FirstName, Is.EqualTo(""));
            user.FirstName = "Robert";
            Assert.That(user.FirstName, Is.EqualTo("Robert"));
        }

        [Test]
        public void Get_and_set_LastName()
        {
            var user = new User();
            Assert.That(user.LastName, Is.EqualTo(""));
            user.LastName = "Baden-Powell";
            Assert.That(user.LastName, Is.EqualTo("Baden-Powell"));
        }
    }
}
