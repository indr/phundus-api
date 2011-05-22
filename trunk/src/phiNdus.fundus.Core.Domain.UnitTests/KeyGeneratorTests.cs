using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace phiNdus.fundus.Core.Domain.UnitTests
{
    [TestFixture]
    class KeyGeneratorTests
    {
        [Test]
        public void CreateKey_does_not_return_same_key_twice()
        {
            var key1 = KeyGenerator.CreateKey(10);
            var key2 = KeyGenerator.CreateKey(10);

            Assert.That(key1, Is.Not.EqualTo(key2));
        }

        [Test]
        public void CreateKey_returns_key_in_lowercase()
        {
            // HINT,Inder: Weils einfach schöner ist... =)
            var key = KeyGenerator.CreateKey(10);
            Assert.That(key, Is.EqualTo(key.ToLower(CultureInfo.CurrentCulture)));
        }

        [Test]
        public void CreateKey_returns_key_of_given_length()
        {
            var key = KeyGenerator.CreateKey(5);
            Assert.That(key, Is.Not.Null);
            Assert.That(key.Length, Is.EqualTo(5));
        }
    }
}
