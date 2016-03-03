namespace Phundus.Tests._Legacy.Entities
{
    using System;
    using System.Threading;
    using NUnit.Framework;
    using Phundus.IdentityAccess.Model.Users;
    using Phundus.IdentityAccess.Users.Exceptions;
    using Phundus.IdentityAccess.Users.Model;

    [TestFixture]
    public class MembershipTests
    {
        [SetUp]
        public void SetUp()
        {
            Sut = new Account();
            Sut.IsApproved = true;
            Sut.IsLockedOut = false;
        }

        protected Account Sut { get; set; }

        private string GetNewSessionId()
        {
            return Guid.NewGuid().ToString("N");
        }

        [Test]
        public void CanGenerateValidationKey()
        {
            Sut.GenerateValidationKey();
            Assert.That(Sut.ValidationKey, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void GenerateValidationKeyReturnsNewValidationKey()
        {
            string old = Sut.ValidationKey;
            Assert.That(Sut.GenerateValidationKey(), Is.Not.EqualTo(old));
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
            string old = Sut.ValidationKey;
            Sut.GenerateValidationKey();
            Assert.That(Sut.ValidationKey, Is.Not.EqualTo(old));
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
        public void LogOn_with_password_null_throws()
        {
            Assert.Throws<ArgumentNullException>(() => Sut.LogOn(GetNewSessionId(), null));
        }

        [Test]
        public void LogOn_with_sessionKey_empty_throws()
        {
            Assert.Throws<ArgumentException>(() => Sut.LogOn("", "1234"));
        }

        [Test]
        public void LogOn_with_sessionKey_null_throws()
        {
            Assert.Throws<ArgumentNullException>(() => Sut.LogOn(null, "1234"));
        }
    }
}