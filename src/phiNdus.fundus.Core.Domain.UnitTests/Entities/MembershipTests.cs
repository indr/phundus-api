using System;
using System.Threading;
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

        [Test]
        public void Set_Password_and_get_Password_are_not_equal()
        {
            Sut.Password = "1234";
            Assert.That(Sut.Password, Is.Not.EqualTo("1234"));
        }

        [Test]
        public void Set_same_password_to_different_memberships_results_in_different_encrypted_password()
        {
            var membership1 = new Membership();
            var membership2 = new Membership();
            membership1.Password = "1234";
            membership2.Password = "1234";
            Assert.That(membership2.Password, Is.Not.EqualTo(membership1.Password));
        }

        [Test]
        public void Set_Password_updates_LastPasswordChange()
        {
            Sut.Password = "new Password";
            Assert.That(Sut.LastPasswordChangeDate, Is.EqualTo(DateTime.Now).Within(1).Seconds);
        }

        [Test]
        public void Set_same_password_twice_does_not_update_LastPasswordChange()
        {
            Sut.Password = "Password";
            var firstSet = DateTime.Now;
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Sut.Password = "Password";
            Assert.That(Sut.LastPasswordChangeDate, Is.EqualTo(firstSet).Within(1).Seconds);
        }

        [Test]
        public void LockOut_updates_LastLockedOutDate()
        {
            Sut.LockOut();
            Assert.That(Sut.IsLockedOut, Is.True);
            Assert.That(Sut.LastLockoutDate, Is.EqualTo(DateTime.Now).Within(1).Seconds);
        }
    }
}