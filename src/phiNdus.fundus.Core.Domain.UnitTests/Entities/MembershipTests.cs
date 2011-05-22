using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    internal class MembershipTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new Membership();
            Sut.Password = "1234";
            Sut.IsApproved = true;
            Sut.IsLockedOut = false;
        }

        #endregion

        protected Membership Sut { get; set; }

        [Test]
        public void LogOn_sets_SessionKey_with_length_20()
        {
            Assert.That(Sut.SessionKey, Is.Null);
            Sut.LogOn("1234");
            Assert.That(Sut.SessionKey, Is.Not.Null);
            Assert.That(Sut.SessionKey.Length, Is.EqualTo(20));
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
            var oldSessionKey = Sut.SessionKey;
            Assert.That(Sut.SessionKey, Is.Null);
            Sut.LogOn("1234");
            Assert.That(Sut.SessionKey, Is.Not.Null);
            Assert.That(Sut.SessionKey, Is.Not.EqualTo(oldSessionKey));
        }

        [Test]
        public void LogOn_when_locked_out_throws()
        {
            Sut.IsLockedOut = true;
            Assert.Throws<UserLockedOutException>(() => Sut.LogOn(""));
        }

        [Test]
        public void LogOn_when_not_approved_throws()
        {
            Sut.IsApproved = false;
            Assert.Throws<UserNotApprovedException>(() => Sut.LogOn(""));
        }

        [Test]
        public void LogOn_with_invalid_password_throws()
        {
            Sut.Password = "1234";
            Assert.Throws<InvalidPasswordException>(() => Sut.LogOn("4321"));
        }

        [Test]
        public void LogOn_with_password_null_throws()
        {
            Assert.Throws<ArgumentNullException>(() => Sut.LogOn(null));
        }
    }
}