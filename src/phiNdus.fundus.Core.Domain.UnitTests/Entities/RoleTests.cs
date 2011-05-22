using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    internal class RoleTests
    {
        [Test]
        public void Can_create()
        {
            new Role();
        }

        [Test]
        public void Equals_by_name()
        {
            var role1 = new Role();
            role1.Name = "User";
            var role2 = new Role();
            role2.Name = "User";

            Assert.That(role1.Equals(role2), Is.True);
        }

        [Test]
        public void Get_Administrator_role()
        {
            var role = Role.Administrator;
            Assert.That(role, Is.Not.Null);
            Assert.That(role.Id, Is.EqualTo(2));
        }

        [Test]
        public void Get_Id()
        {
            var sut = new Role();
            Assert.That(sut.Id, Is.EqualTo(0));
        }

        [Test]
        public void Get_User_role()
        {
            var role = Role.User;
            Assert.That(role, Is.Not.Null);
            Assert.That(role.Id, Is.EqualTo(1));
        }

        [Test]
        public void Set_and_get_Name()
        {
            var sut = new Role();
            Assert.That(sut.Name, Is.Null);
            sut.Name = "Benutzer";
            Assert.That(sut.Name, Is.EqualTo("Benutzer"));
        }
    }
}