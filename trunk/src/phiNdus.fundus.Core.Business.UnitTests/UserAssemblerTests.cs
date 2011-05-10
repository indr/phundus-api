﻿using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Domain;

namespace phiNdus.fundus.Core.Business.UnitTests
{
    [TestFixture]
    public class UserAssemblerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _assembler = new UserAssembler();
            _user = new User(1);
            _user.FirstName = "John";
            _user.LastName = "Wayne";
            _user.Membership = new Membership();
            _user.Membership.CreateDate = new DateTime(2011, 6, 5, 14, 48, 55);
            _user.Membership.Email = "john.wayne@example.com";
            _user.Membership.IsApproved = true;
            _user.Membership.PasswordQuestion = "Who really cares?";
        }

        #endregion

        private UserAssembler _assembler;
        private User _user;

        [Test]
        public void WriteDto_returns_correct_dto()
        {
            var dto = _assembler.WriteDto(_user);
            Assert.That(dto.Id, Is.EqualTo(1));
            Assert.That(dto.FirstName, Is.EqualTo("John"));
            Assert.That(dto.LastName, Is.EqualTo("Wayne"));
            Assert.That(dto.Email, Is.EqualTo("john.wayne@example.com"));
            Assert.That(dto.CreateDate, Is.EqualTo(new DateTime(2011, 6, 5, 14, 48, 55)));
            Assert.That(dto.IsApproved, Is.True);
            Assert.That(dto.PasswordQuestion, Is.EqualTo("Who really cares?"));
        }
    }
}