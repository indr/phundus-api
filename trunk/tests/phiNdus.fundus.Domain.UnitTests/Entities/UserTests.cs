namespace phiNdus.fundus.Domain.UnitTests.Entities
{
    using System;
    using NUnit.Framework;
    using Phundus.Core.IdentityAndAccess.Users.Model;

    [TestFixture]
    public class UserTests
    {
        [Test]
        public void CanSetJsNumberWithSevenDigits()
        {
            var sut = new User();
            sut.JsNumber = 1234567;
            Assert.That(sut.JsNumber, Is.EqualTo(1234567));
        }

        [Test]
        public void CanSetJsNumberWithSixDigits()
        {
            var sut = new User();
            sut.JsNumber = 123456;
            Assert.That(sut.JsNumber, Is.EqualTo(123456));
        }

        [Test]
        public void Can_create()
        {
            new User();
        }

        [Test]
        public void Can_get_DisplayName()
        {
            var sut = new User();
            sut.FirstName = "Hans";
            sut.LastName = "Wahrig";
            Assert.That(sut.DisplayName, Is.EqualTo("Hans Wahrig"));
        }

        [Test]
        public void Can_set_and_get_FirstName()
        {
            var sut = new User();
            Assert.That(sut.FirstName, Is.EqualTo(""));
            sut.FirstName = "Robert";
            Assert.That(sut.FirstName, Is.EqualTo("Robert"));
        }

        [Test]
        public void Can_set_and_get_LastName()
        {
            var sut = new User();
            Assert.That(sut.LastName, Is.EqualTo(""));
            sut.LastName = "Baden-Powell";
            Assert.That(sut.LastName, Is.EqualTo("Baden-Powell"));
        }

        [Test]
        public void Can_set_and_get_Role()
        {
            var role = new Role();
            var sut = new User();
            sut.Role = role;
            Assert.That(sut.Role, Is.EqualTo(role));
        }

        [Test]
        public void Create_assignes_Membership()
        {
            var sut = new User();
            Assert.That(sut.Account, Is.Not.Null);
            Assert.That(sut.Account.User, Is.EqualTo(sut));
        }

        [Test]
        public void Create_assignes_User_role()
        {
            var sut = new User();
            Assert.That(sut.Role, Is.Not.Null);
            Assert.That(sut.Role, Is.EqualTo(Role.User));
        }

        [Test]
        public void Set_JsNumber_less_than_six_numbers_throws()
        {
            var sut = new User();
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sut.JsNumber = 0);
            Assert.That(ex.ParamName, Is.EqualTo("value"));
            Assert.That(ex.Message, Is.StringStarting("Die J+S-Nummer muss sechs- oder siebenstellig sein."));
        }

        [Test]
        public void Set_JsNumber_more_than_seven_numbers_throws()
        {
            var sut = new User();
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sut.JsNumber = 10000000);
            Assert.That(ex.ParamName, Is.EqualTo("value"));
            Assert.That(ex.Message, Is.StringStarting("Die J+S-Nummer muss sechs- oder siebenstellig sein."));
        }
    }
}