namespace Phundus.Core.Tests._Legacy.Entities
{
    using System;
    using Core.IdentityAndAccess.Users.Model;
    using NUnit.Framework;

    [TestFixture]
    public class UserTests
    {
        protected static User CreateUser()
        {
            return new User("user@test.phundus.ch", "1234", "Hans", "Müller", "Street", "1234", "City",
                "012 345 67 89", null);
        }

        [Test]
        public void CanSetJsNumberWithSevenDigits()
        {
            var sut = CreateUser();
            sut.JsNumber = 1234567;
            Assert.That(sut.JsNumber, Is.EqualTo(1234567));
        }

        [Test]
        public void CanSetJsNumberWithSixDigits()
        {
            var sut = CreateUser();
            sut.JsNumber = 123456;
            Assert.That(sut.JsNumber, Is.EqualTo(123456));
        }

        [Test]
        public void Can_get_DisplayName()
        {
            var sut = CreateUser();
            sut.FirstName = "Hans";
            sut.LastName = "Wahrig";
            Assert.That(sut.DisplayName, Is.EqualTo("Hans Wahrig"));
        }

        [Test]
        public void Can_set_and_get_FirstName()
        {
            var sut = CreateUser();
            Assert.That(sut.FirstName, Is.EqualTo("Hans"));
            sut.FirstName = "Robert";
            Assert.That(sut.FirstName, Is.EqualTo("Robert"));
        }

        [Test]
        public void Can_set_and_get_LastName()
        {
            var sut = CreateUser();
            Assert.That(sut.LastName, Is.EqualTo("Müller"));
            sut.LastName = "Baden-Powell";
            Assert.That(sut.LastName, Is.EqualTo("Baden-Powell"));
        }

        [Test]
        public void Create_assignes_Membership()
        {
            var sut = CreateUser();
            Assert.That(sut.Account, Is.Not.Null);
            Assert.That(sut.Account.User, Is.EqualTo(sut));
        }

        [Test]
        public void Set_JsNumber_less_than_six_numbers_throws()
        {
            var sut = CreateUser();
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sut.JsNumber = 0);
            Assert.That(ex.ParamName, Is.EqualTo("value"));
            Assert.That(ex.Message, Is.StringStarting("Die J+S-Nummer muss sechs- oder siebenstellig sein."));
        }

        [Test]
        public void Set_JsNumber_more_than_seven_numbers_throws()
        {
            var sut = CreateUser();
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => sut.JsNumber = 10000000);
            Assert.That(ex.ParamName, Is.EqualTo("value"));
            Assert.That(ex.Message, Is.StringStarting("Die J+S-Nummer muss sechs- oder siebenstellig sein."));
        }
    }
}