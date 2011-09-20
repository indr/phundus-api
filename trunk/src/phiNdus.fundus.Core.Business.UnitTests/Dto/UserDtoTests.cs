using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;

namespace phiNdus.fundus.Core.Business.UnitTests.Dto
{
    [TestFixture]
    public class UserDtoTests
    {
        [Test]
        public void Can_create()
        {
            var sut = new UserDto();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Can_get_and_set_CreateDate()
        {
            var now = DateTime.Now;
            var sut = new UserDto();
            sut.CreateDate = now;
            Assert.That(sut.CreateDate, Is.EqualTo(now));
        }

        [Test]
        public void Can_get_and_set_Email()
        {
            var sut = new UserDto();
            sut.Email = "dave@example.com";
            Assert.That(sut.Email, Is.EqualTo("dave@example.com"));
        }

        [Test]
        public void Can_get_and_set_FirstName()
        {
            var sut = new UserDto();
            sut.FirstName = "Dave";
            Assert.That(sut.FirstName, Is.EqualTo("Dave"));
        }

        [Test]
        public void Can_get_and_set_Id()
        {
            var sut = new UserDto();
            sut.Id = 1;
            Assert.That(sut.Id, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_IsApproved()
        {
            var sut = new UserDto();
            sut.IsApproved = true;
            Assert.That(sut.IsApproved, Is.True);
        }

        [Test]
        public void Can_get_and_set_LastName()
        {
            var sut = new UserDto();
            sut.LastName = "Example";
            Assert.That(sut.LastName, Is.EqualTo("Example"));
        }

        [Test]
        public void Can_get_and_set_Version()
        {
            var sut = new UserDto();
            sut.Version = 1;
            Assert.That(sut.Version, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_RoleId()
        {
            var sut = new UserDto();
            sut.RoleId = 1;
            Assert.That(sut.RoleId, Is.EqualTo(1));
        }

        [Test]
        public void Can_get_and_set_RoleName()
        {
            var sut = new UserDto();
            sut.RoleName = "User";
            Assert.That(sut.RoleName, Is.EqualTo("User"));
        }
    }
}