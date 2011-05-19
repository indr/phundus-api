using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    class UserTests
    {
        [Test]
        public void Can_create()
        {
            new User();
        }

        [Test]
        public void Constructor_assignes_Membership()
        {
            var user = new User();
            Assert.That(user.Membership, Is.Not.Null);
            Assert.That(user.Membership.User, Is.EqualTo(user));
        }

        [Test]
        public void Can_set_and_get_FirstName()
        {
            var user = new User();
            Assert.That(user.FirstName, Is.EqualTo(""));
            user.FirstName = "Robert";
            Assert.That(user.FirstName, Is.EqualTo("Robert"));
        }

        [Test]
        public void Can_set_and_get_LastName()
        {
            var user = new User();
            Assert.That(user.LastName, Is.EqualTo(""));
            user.LastName = "Baden-Powell";
            Assert.That(user.LastName, Is.EqualTo("Baden-Powell"));
        }
    }
}
