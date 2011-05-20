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
        public void Get_Id()
        {
            var sut = new Role();
            Assert.That(sut.Id, Is.EqualTo(0));
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