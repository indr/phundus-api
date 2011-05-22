using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    internal class MembershipTests
    {
        [SetUp]
        public void SetUp()
        {
            Sut = new Membership();
            Sut.Password = "1234";
        }

        protected Membership Sut { get; set; }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LogOn_with_password_null_throws()
        {
            Sut.LogOn(null);
        }

        [Test]
        public void LogOn_updates_LastLogOnDate()
        {
            Assert.That(Sut.LastLogOnDate, Is.Null);
            Sut.LogOn("1234");
            Assert.That(Sut.LastLogOnDate, Is.EqualTo(DateTime.Now).Within(1).Seconds);
        }

        [Test]
        public void LogOn_updates_SessionKey()
        {
            Assert.That(Sut.SessionKey, Is.Null);
            Sut.LogOn("1234");
            Assert.That(Sut.SessionKey, Is.Not.Null);
        }

        [Test]
        [ExpectedException(typeof(UserLookedOutException))]
        public void LogOn_when_looked_out_throws()
        {
            Sut.IsLockedOut = true;
            Sut.LogOn("");
        }

        [Test]
        [ExpectedException(typeof(InvalidPasswordException))]
        public void LogOn_with_invalid_password_throws()
        {
            Sut.Password = "1234";
            Sut.LogOn("4321");
        }
    }
}