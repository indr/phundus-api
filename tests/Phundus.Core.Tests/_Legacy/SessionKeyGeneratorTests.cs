namespace Phundus.Tests._Legacy
{
    using System.Globalization;
    using NUnit.Framework;
    using Phundus.IdentityAccess.Users.Services;

    [TestFixture]
    public class SessionKeyGeneratorTests
    {
        [Test]
        public void CreateKey_does_not_return_same_key_twice()
        {
            var sessionKey1 = SessionKeyGenerator.CreateKey();
            var sessionKey2 = SessionKeyGenerator.CreateKey();

            Assert.That(sessionKey1, Is.Not.EqualTo(sessionKey2));
        }

        [Test]
        public void CreateKey_returns_key_in_lowercase()
        {
            // HINT,Inder: Weils einfach schöner ist... =)
            var sessionKey = SessionKeyGenerator.CreateKey();
            Assert.That(sessionKey, Is.EqualTo(sessionKey.ToLower(CultureInfo.CurrentCulture)));
        }

        [Test]
        public void CreateKey_returns_key_of_length_20()
        {
            var sessionKey = SessionKeyGenerator.CreateKey();
            Assert.That(sessionKey, Is.Not.Null);
            Assert.That(sessionKey.Length, Is.EqualTo(20));
        }
    }
}