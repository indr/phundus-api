using NUnit.Framework;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Domain.UnitTests.Entities
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void Can_create()
        {
            new User();
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
        public void Can_get_DisplayName()
        {
            var sut = new User();
            sut.FirstName = "Hans";
            sut.LastName = "Wahrig";
            Assert.That(sut.DisplayName, Is.EqualTo("Hans Wahrig"));
        }

        [Test]
        public void Can_set_and_get_Role()
        {
            var role = new Role();
            var sut = new User();
            sut.Role = role;
            Assert.That(sut.Role, Is.EqualTo(role));
        }

        [Test]
        public void Create_assignes_Membership()
        {
            var sut = new User();
            Assert.That(sut.Membership, Is.Not.Null);
            Assert.That(sut.Membership.User, Is.EqualTo(sut));
        }

        [Test]
        public void Create_assignes_User_role()
        {
            var sut = new User();
            Assert.That(sut.Role, Is.Not.Null);
            Assert.That(sut.Role, Is.EqualTo(Role.User));
        }
    }
}