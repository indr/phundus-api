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
            var sut = new User();
            Assert.That(sut.Membership, Is.Not.Null);
            Assert.That(sut.Membership.User, Is.EqualTo(sut));
        }

        [Test]
        public void Can_set_and_get_FirstName()
        {
            var sut = new User();
            Assert.That(sut.FirstName, Is.EqualTo(""));
            sut.FirstName = "Robert";
            Assert.That(sut.FirstName, Is.EqualTo("Robert"));
        }

        [Test]
        public void Can_set_and_get_LastName()
        {
            var sut = new User();
            Assert.That(sut.LastName, Is.EqualTo(""));
            sut.LastName = "Baden-Powell";
            Assert.That(sut.LastName, Is.EqualTo("Baden-Powell"));
        }

        [Test]
        public void Can_set_and_get_Role()
        {
            var role = new Role();
            var sut = new User();
            Assert.That(sut.Role, Is.Null);
            sut.Role = role;
            Assert.That(sut.Role, Is.EqualTo(role));
        }
    }
}
