using System;
using System.Threading;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Domain.UnitTests.Entities
{
    [TestFixture]
    public class MembershipTests
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

        private string GetNewSessionId()
        {
            return Guid.NewGuid().ToString("N");
        }

        [Test]
        public void LogOn_updates_LastLogOnDate()
        {
            Assert.That(Sut.LastLogOnDate, Is.Null);
            Sut.LogOn(GetNewSessionId(), "1234");
            Assert.That(Sut.LastLogOnDate, Is.EqualTo(DateTime.Now).Within(1).Seconds);
        }

        [Test]
        public void LogOn_updates_SessionKey()
        {
            var sessionKey = GetNewSessionId();
            Assert.That(Sut.SessionKey, Is.Null);
            Sut.LogOn(sessionKey, "1234");
            Assert.That(Sut.SessionKey, Is.EqualTo(sessionKey));
        }

        [Test]
        public void LogOn_when_locked_out_throws()
        {
            Sut.IsLockedOut = true;
            Assert.Throws<UserLockedOutException>(() => Sut.LogOn(GetNewSessionId(), ""));
        }

        [Test]
        public void LogOn_when_not_approved_throws()
        {
            Sut.IsApproved = false;
            Assert.Throws<UserNotApprovedException>(() => Sut.LogOn(GetNewSessionId(), ""));
        }

        [Test]
        public void LogOn_with_invalid_password_throws()
        {
            Sut.Password = "1234";
            Assert.Throws<InvalidPasswordException>(() => Sut.LogOn(GetNewSessionId(), "4321"));
        }

        [Test]
        public void LogOn_with_password_null_throws()
        {
            Assert.Throws<ArgumentNullException>(() => Sut.LogOn(GetNewSessionId(), null));
        }

        [Test]
        public void LogOn_with_sessionKey_null_throws()
        {
            Assert.Throws<ArgumentNullException>(() => Sut.LogOn(null, "1234"));
        }

        [Test]
        public void LogOn_with_sessionKey_empty_throws()
        {
            Assert.Throws<ArgumentException>(() => Sut.LogOn("", "1234"));
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

        [Test]
        public void CanGenerateValidationKey()
        {
            Sut.GenerateValidationKey();
            Assert.That(Sut.ValidationKey, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void GenerateValidationKeySetKeyWithLengthOf24()
        {
            Sut.GenerateValidationKey();
            Assert.That(Sut.ValidationKey.Length, Is.EqualTo(24));
        }

        [Test]
        public void GenerateValidationKeySetsNewKey()
        {
            Sut.GenerateValidationKey();
            var old = Sut.ValidationKey;
            Sut.GenerateValidationKey();
            Assert.That(Sut.ValidationKey, Is.Not.EqualTo(old));
        }

        [Test]
        public void GenerateValidationKeyReturnsNewValidationKey()
        {
            var old = Sut.ValidationKey;
            Assert.That(Sut.GenerateValidationKey(), Is.Not.EqualTo(old));
        }
    }
}