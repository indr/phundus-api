﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace phiNdus.fundus.Core.Domain.UnitTests
{
    [TestFixture]
    public class PasswordEncryptorTests
    {
        [Test]
        public void Encode_with_value_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => PasswordEncryptor.Encrypt(null, "Salt"));
            Assert.That(ex.ParamName, Is.EqualTo("value"));
        }

        [Test]
        public void Encode_with_salt_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => PasswordEncryptor.Encrypt("", null));
            Assert.That(ex.ParamName, Is.EqualTo("salt"));
        }

        [Test]
        public void Encode_with_different_salts_return_different_values()
        {
            var encrypted1 = PasswordEncryptor.Encrypt("1234", "Salt1");
            var encrypted2 = PasswordEncryptor.Encrypt("1234", "Salt2");

            Assert.That(encrypted2, Is.Not.EqualTo(encrypted1));
        }

        [Test]
        public void Encode_removes_dashes()
        {
            var encrypted = PasswordEncryptor.Encrypt("1234", "Salt");
            Assert.That(encrypted.IndexOf("-"), Is.EqualTo(-1));
        }
    }
}
