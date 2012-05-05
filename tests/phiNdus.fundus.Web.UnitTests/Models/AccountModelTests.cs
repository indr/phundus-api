using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using phiNdus.fundus.Web.Models;

namespace phiNdus.fundus.Web.UnitTests.Models {
    [TestFixture]
    public class AccountModelTests {

        private const string DefaultPassword = @"nf2èp3in";
        private const string DefaultEmail = @"dave@example.com";
        private const string InvalidEmail = @"dave@example";
        private const string DefaultFistName = @"Dave";
        private const string DefaultLastName = @"Example";

        [Test]
        public void LogOnModel_ensure_that_eMail_is_required() {
            var model = new LogOnModel { Password = DefaultPassword };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate<LogOnModel>(model));

            Assert.That(exception.Message, Contains.Substring(@"Email").And.Contains(@"erforderlich"));
        }

        [Test]
        public void LogOnModel_ensure_that_eMail_has_to_be_valid() {
            var model = new LogOnModel { 
                Email = InvalidEmail, 
                Password = DefaultPassword 
            };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate<LogOnModel>(model));

            Assert.That(exception.Message, Contains.Substring(@"E-Mail").And.Contains(@"Ungültige"));
        }

        [Test]
        public void LogOnModel_ensure_that_password_is_required() {
            var model = new LogOnModel { Email = DefaultEmail };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate<LogOnModel>(model));

            Assert.That(exception.Message, Contains.Substring(@"Password").And.Contains(@"erforderlich"));
        }

        [Test]
        public void LogOnModel_ensure_that_model_validates() {
            var model = new LogOnModel { 
                Email = DefaultEmail, 
                Password = DefaultPassword 
            };

            ModelValidator.Validate<LogOnModel>(model);
        }

        [Test]
        public void SignUpModel_ensure_that_eMail_is_required() {
            var model = new SignUpModel { 
                FirstName = DefaultFistName,
                LastName = DefaultLastName,
                Password = DefaultPassword
            };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate<SignUpModel>(model));

            Assert.That(exception.Message, Contains.Substring(@"Email").And.Contains(@"erforderlich"));
        }

        [Test]
        public void SignUpModel_ensure_that_eMail_has_to_be_valid() {
            var model = new SignUpModel {
                Email = InvalidEmail,
                FirstName = DefaultFistName,
                LastName = DefaultLastName,
                Password = DefaultPassword
            };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate<SignUpModel>(model));

            Assert.That(exception.Message, Contains.Substring(@"E-Mail").And.Contains(@"Ungültige"));
        }

        [Test]
        public void SignUpModel_ensure_that_firstname_is_required() {
            var model = new SignUpModel {
                Email = DefaultEmail,
                LastName = DefaultLastName,
                Password = DefaultPassword
            };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate<SignUpModel>(model));

            Assert.That(exception.Message, Contains.Substring(@"FirstName").And.Contains(@"erforderlich"));
        }

        [Test]
        public void SignUpModel_ensure_that_lastname_is_required() {
            var model = new SignUpModel {
                Email = DefaultEmail,
                FirstName = DefaultFistName,
                Password = DefaultPassword
            };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate<SignUpModel>(model));

            Assert.That(exception.Message, Contains.Substring(@"LastName").And.Contains(@"erforderlich"));
        }

        [Test]
        public void SignUpModel_ensure_that_password_is_required() {
            var model = new SignUpModel {
                Email = DefaultEmail,
                FirstName = DefaultFistName,
                LastName = DefaultLastName
                
            };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate<SignUpModel>(model));

            Assert.That(exception.Message, Contains.Substring(@"Password").And.Contains(@"erforderlich"));
        }

        [Test]
        public void SignUpModel_ensures_that_jsNumber_is_required()
        {
            var model = new SignUpModel
            {
                Email = DefaultEmail,
                FirstName = DefaultFistName,
                LastName = DefaultLastName,
                Password = DefaultPassword
            };

            var exception = Assert.Throws<ValidationException>(
                () => ModelValidator.Validate<SignUpModel>(model));

            Assert.That(exception.Message, Contains.Substring(@"JsNumber").And.Contains(@"zwischen 1 und 999999 liegen"));
        }

        [Test]
        public void SignUpModel_ensure_that_model_validates() {
            var model = new SignUpModel {
                Email = DefaultEmail,
                FirstName = DefaultFistName,
                LastName = DefaultLastName,
                Password = DefaultPassword,
                JsNumber = 1
            };

            ModelValidator.Validate<SignUpModel>(model);
        }
    }
}
