
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Web.Security;
using phiNdus.fundus.Core.Business;
using Rhino.Mocks;
using System.Web.Security;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace phiNdus.fundus.Core.Web.UnitTests.Security {

    [TestFixture]
    public class FundusMembershipProviderTests : MockTestBase<FundusMembershipProvider> {

        private IUserService UserServiceMock { get; set; }

        protected override void RegisterDependencies(IWindsorContainer container) {
            base.RegisterDependencies(container);

            this.UserServiceMock = this.MockFactory.StrictMock<IUserService>();

            container.Register(Component.For<IUserService>().Instance(this.UserServiceMock));
        }

        protected override FundusMembershipProvider CreateSut() {
            var provider = new FundusMembershipProvider();
                        
            provider.Initialize("FundusMembershipProvider",
                new System.Collections.Specialized.NameValueCollection {
                    { "enablePasswordReset", "true" },
                    { "enablePasswordRetrieval", "false" },
                    { "applicationName", "fundus" },
                    { "maxInvalidPasswordAttempts", "5" },
                    { "minRequiredPasswordLength", "8" },
                    { "minRequiredNonAlphanumericCharacters", "3" },
                    { "passwordAttemptWindow", "10" }
                });

            return provider;
        }
        
        [Test]
        public void Ensure_password_reset_is_set_according_to_the_configuration() {
            Assert.That(this.Sut.EnablePasswordReset, Is.True);
        }

        [Test]
        public void Ensure_password_retrieval_is_set_according_to_the_configuration() {
            Assert.That(this.Sut.EnablePasswordRetrieval, Is.False);
        }

        [Test]
        public void Ensure_application_name_is_set_according_to_the_configuration() {
            Assert.That(this.Sut.ApplicationName, Is.EqualTo("fundus"));
        }

        [Test]
        public void Ensure_max_invalid_password_attempts_is_set_according_to_the_configuration() {
            Assert.That(this.Sut.MaxInvalidPasswordAttempts, Is.EqualTo(5));
        }

        [Test]
        public void Ensure_min_required_password_length_is_set_according_to_the_configuration() {
            Assert.That(this.Sut.MinRequiredPasswordLength, Is.EqualTo(8));
        }

        [Test]
        public void Ensure_min_required_non_alphanumeric_characters_is_set_according_to_the_configuration() {
            Assert.That(this.Sut.MinRequiredNonAlphanumericCharacters, Is.EqualTo(3));
        }

        [Test]
        public void Ensure_password_attempt_window_is_set_according_to_the_configuration() {
            Assert.That(this.Sut.PasswordAttemptWindow, Is.EqualTo(10));
        }

        [Test]
        public void Changing_the_password_should_relay_action_to_business_layer() {
            var email = "john.doe@google.com";
            var oldPassword = "23ioN09*c$sE";
            var newPassword = "Nlwä2$_n32#@";
            var passwordChanged = false;

            With.Mocks(this.MockFactory).Expecting(delegate {
                Expect.Call(this.UserServiceMock.ChangePassword(null, email, oldPassword, newPassword))
                    .Return(true);
            }).Verify(delegate {
                passwordChanged = this.Sut.ChangePassword(email, oldPassword, newPassword);
            });

            Assert.That(passwordChanged, Is.True);
        }

        [Test]
        public void Creating_a_new_user_should_relay_action_to_business_layer() { 
            var username = "john.doe@google.com";
            var password = "Nlwä2$_n32#@";
            var passwordQuestion = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr?";
            var passwordAnswer = "sed diam nonumy";
            var isApproved = false;

            MembershipCreateStatus status = (MembershipCreateStatus)(-1);
            MembershipUser createdUser = null;
            var creationDate = DateTime.Now;

            With.Mocks(this.MockFactory).Expecting(delegate {
                Expect.Call(this.UserServiceMock.CreateUser(null, username, password))
                    .Return(new UserDto {
                        Email = username,
                        IsApproved = isApproved,
                        CreateDate =creationDate
                    });
            }).Verify(delegate {
                createdUser = this.Sut.CreateUser(username, password, null, passwordQuestion, passwordAnswer, isApproved, null, out status);
            });

            Assert.That(status, Is.EqualTo(MembershipCreateStatus.Success));

            Assert.That(createdUser, Is.Not.Null);
            Assert.That(createdUser.UserName, Is.EqualTo(username));
            Assert.That(createdUser.Email, Is.EqualTo(username));
            Assert.That(createdUser.CreationDate, Is.EqualTo(creationDate));
        }

    }
}
