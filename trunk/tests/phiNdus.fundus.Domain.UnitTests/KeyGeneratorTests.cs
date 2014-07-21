using System.Globalization;
using NUnit.Framework;

namespace phiNdus.fundus.Domain.UnitTests
{
    using Phundus.Core.IdentityAndAccessCtx;

    [TestFixture]
    public class KeyGeneratorTests
    {
        [Test]
        public void CreateKey_does_not_return_same_key_twice()
        {
            string key1 = KeyGenerator.CreateKey(10);
            string key2 = KeyGenerator.CreateKey(10);

            Assert.That(key1, Is.Not.EqualTo(key2));
        }

        [Test]
        public void CreateKey_returns_key_in_lowercase()
        {
            // HINT,Inder: Weils einfach schöner ist... =)
            string key = KeyGenerator.CreateKey(10);
            Assert.That(key, Is.EqualTo(key.ToLower(CultureInfo.CurrentCulture)));
        }

        [Test]
        public void CreateKey_returns_key_of_given_length()
        {
            string key = KeyGenerator.CreateKey(5);
            Assert.That(key, Is.Not.Null);
            Assert.That(key.Length, Is.EqualTo(5));
        }
    }
}