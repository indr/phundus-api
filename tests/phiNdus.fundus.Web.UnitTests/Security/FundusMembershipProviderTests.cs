using System;
using System.Collections.Specialized;
using System.Web.Security;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Business;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Web.Security;
using phiNdus.fundus.TestHelpers;
using Rhino.Mocks;

namespace phiNdus.fundus.Web.UnitTests.Security
{
    [TestFixture]
    public class FundusMembershipProviderTests : MockTestBase<CustomMembershipProvider>
    {
        protected override CustomMembershipProvider CreateSut()
        {
            var provider = new CustomMembershipProvider();

            provider.Initialize("CustomerMembershipProvider",
                                new NameValueCollection
                                    {
                                        {"enablePasswordReset", "true"},
                                        {"enablePasswordRetrieval", "false"},
                                        {"applicationName", "fundus"},
                                        {"maxInvalidPasswordAttempts", "5"},
                                        {"minRequiredPasswordLength", "8"},
                                        {"minRequiredNonAlphanumericCharacters", "3"},
                                        {"passwordAttemptWindow", "10"}
                                    });

            return provider;
        }

        

        

        [Test]
        public void DefaultCreateUserMethodThrows()
        {
            MembershipCreateStatus status;
            Assert.Throws<InvalidOperationException>(() => Sut.CreateUser("", "", "", "", "", false, null, out status));
        }

        [Test]
        public void Ensure_application_name_is_set_according_to_the_configuration()
        {
            Assert.That(Sut.ApplicationName, Is.EqualTo("fundus"));
        }

        [Test]
        public void Ensure_max_invalid_password_attempts_is_set_according_to_the_configuration()
        {
            Assert.That(Sut.MaxInvalidPasswordAttempts, Is.EqualTo(5));
        }

        [Test]
        public void Ensure_min_required_non_alphanumeric_characters_is_set_according_to_the_configuration()
        {
            Assert.That(Sut.MinRequiredNonAlphanumericCharacters, Is.EqualTo(3));
        }

        [Test]
        public void Ensure_min_required_password_length_is_set_according_to_the_configuration()
        {
            Assert.That(Sut.MinRequiredPasswordLength, Is.EqualTo(8));
        }

        [Test]
        public void Ensure_password_attempt_window_is_set_according_to_the_configuration()
        {
            Assert.That(Sut.PasswordAttemptWindow, Is.EqualTo(10));
        }

        [Test]
        public void Ensure_password_reset_is_set_according_to_the_configuration()
        {
            Assert.That(Sut.EnablePasswordReset, Is.True);
        }

        [Test]
        public void Ensure_password_retrieval_is_set_according_to_the_configuration()
        {
            Assert.That(Sut.EnablePasswordRetrieval, Is.False);
        }
    }
}