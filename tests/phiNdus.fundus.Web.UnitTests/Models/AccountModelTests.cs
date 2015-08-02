namespace phiNdus.fundus.Web.UnitTests.Models
{
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;
    using Web.Models;

    [TestFixture]
    public class AccountModelTests
    {
        private const string DefaultPassword = @"nf2èp3in";
        private const string DefaultEmail = @"dave@example.com";
        private const string InvalidEmail = @"dave@example";
        private const string DefaultFistName = @"Dave";
        private const string DefaultLastName = @"Example";
        private const string DefaultStreet = @"Street";
        private const string DefaultPostcode = @"1000";
        private const string DefaultCity = @"City";
        private const string DefaultMobilePhone = @"0790001122";

        [Test]
        public void LogOnModel_ensure_that_eMail_has_to_be_valid()
        {
            var model = new LogOnModel
            {
                Email = InvalidEmail,
                Password = DefaultPassword
            };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate(model));

            Assert.That(exception.Message, Contains.Substring(@"E-Mail").And.Contains(@"Ungültige"));
        }

        [Test]
        public void LogOnModel_ensure_that_eMail_is_required()
        {
            var model = new LogOnModel {Password = DefaultPassword};

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate(model));

            Assert.That(exception.Message, Contains.Substring(@"Email").And.Contains(@"erforderlich"));
        }

        [Test]
        public void LogOnModel_ensure_that_model_validates()
        {
            var model = new LogOnModel
            {
                Email = DefaultEmail,
                Password = DefaultPassword
            };

            ModelValidator.Validate(model);
        }

        [Test]
        public void LogOnModel_ensure_that_password_is_required()
        {
            var model = new LogOnModel {Email = DefaultEmail};

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate(model));

            Assert.That(exception.Message, Contains.Substring(@"Password").And.Contains(@"erforderlich"));
        }

        [Test]
        public void SignUpModel_ensure_that_eMail_has_to_be_valid()
        {
            var model = new SignUpModel
            {
                Email = InvalidEmail,
                FirstName = DefaultFistName,
                LastName = DefaultLastName,
                Password = DefaultPassword
            };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate(model));

            Assert.That(exception.Message, Contains.Substring(@"E-Mail").And.Contains(@"Ungültige"));
        }

        [Test]
        public void SignUpModel_ensure_that_eMail_is_required()
        {
            var model = new SignUpModel
            {
                FirstName = DefaultFistName,
                LastName = DefaultLastName,
                Password = DefaultPassword
            };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate(model));

            Assert.That(exception.Message, Contains.Substring(@"Email").And.Contains(@"erforderlich"));
        }

        [Test]
        public void SignUpModel_ensure_that_firstname_is_required()
        {
            var model = new SignUpModel
            {
                Email = DefaultEmail,
                LastName = DefaultLastName,
                MobilePhone = DefaultMobilePhone,
                Street = DefaultStreet,
                Postcode = DefaultPostcode,
                City = DefaultCity,
                Password = DefaultPassword,
                PasswordAgain = DefaultPassword,
            };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate(model));

            Assert.That(exception.Message, Contains.Substring(@"FirstName").And.Contains(@"erforderlich"));
        }

        [Test]
        public void SignUpModel_ensure_that_lastname_is_required()
        {
            var model = new SignUpModel
            {
                Email = DefaultEmail,
                FirstName = DefaultFistName,
                MobilePhone = DefaultMobilePhone,
                Street = DefaultStreet,
                Postcode = DefaultPostcode,
                City = DefaultCity,
                Password = DefaultPassword,
                PasswordAgain = DefaultPassword,
            };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate(model));

            Assert.That(exception.Message, Contains.Substring(@"LastName").And.Contains(@"erforderlich"));
        }

        [Test]
        public void SignUpModel_ensure_that_model_validates()
        {
            var model = new SignUpModel
            {
                Email = DefaultEmail,
                FirstName = DefaultFistName,
                LastName = DefaultLastName,
                MobilePhone = DefaultMobilePhone,
                Street = DefaultStreet,
                Postcode = DefaultPostcode,
                City = DefaultCity,
                Password = DefaultPassword,
                PasswordAgain = DefaultPassword
            };

            ModelValidator.Validate(model);
        }

        [Test]
        public void SignUpModel_ensure_that_password_is_required()
        {
            var model = new SignUpModel
            {
                Email = DefaultEmail,
                FirstName = DefaultFistName,
                LastName = DefaultLastName
            };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate(model));

            Assert.That(exception.Message, Contains.Substring(@"Password").And.Contains(@"erforderlich"));
        }
    }
}