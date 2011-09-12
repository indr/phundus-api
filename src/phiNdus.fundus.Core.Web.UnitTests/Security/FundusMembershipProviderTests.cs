using System;
using System.Collections.Specialized;
using System.Web.Security;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Business;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Web.Security;
using phiNdus.fundus.TestHelpers;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Web.UnitTests.Security
{
    [TestFixture]
    public class FundusMembershipProviderTests : MockTestBase<FundusMembershipProvider>
    {
        private IUserService MockUserService { get; set; }

        protected override void RegisterDependencies(IWindsorContainer container)
        {
            base.RegisterDependencies(container);

            MockUserService = MockFactory.StrictMock<IUserService>();

            container.Register(Component.For<IUserService>().Instance(MockUserService));
        }

        protected override FundusMembershipProvider CreateSut()
        {
            var provider = new FundusMembershipProvider();

            provider.Initialize("FundusMembershipProvider",
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
        public void Changing_the_password_should_relay_action_to_business_layer()
        {
            Assert.Ignore("HttpContext.Current faken.");

            var email = "john.doe@google.com";
            var oldPassword = "23ioN09*c$sE";
            var newPassword = "Nlwä2$_n32#@";
            var passwordChanged = false;

            With.Mocks(MockFactory).Expecting(delegate
                                                  {
                                                      Expect.Call(MockUserService.ChangePassword(null, email,
                                                                                                 oldPassword,
                                                                                                 newPassword))
                                                          .Return(true);
                                                  }).Verify(
                                                      delegate
                                                          {
                                                              passwordChanged = Sut.ChangePassword(email, oldPassword,
                                                                                                   newPassword);
                                                          });

            Assert.That(passwordChanged, Is.True);
        }

        [Test]
        public void CreateUser_sets_status_when_business_layer_throws_EmailAlreadyTaken()
        {
            Assert.Ignore("HttpContext.Current faken.");

            using (MockFactory.Record())
            {
                Expect.Call(MockUserService.CreateUser(null, "dave@example.com", "1234", "", "")).Throw(
                    new EmailAlreadyTakenException());
            }

            using (MockFactory.Playback())
            {
                MembershipCreateStatus status;
                Sut.CreateUser("dave@example.com", "1234", "", "", out status);
                Assert.That(status, Is.EqualTo(MembershipCreateStatus.DuplicateEmail));
            }
        }

        [Test]
        public void DefaultCreateUserMethodThrows()
        {
            MembershipCreateStatus status;
            Assert.Throws<InvalidOperationException>(() => Sut.CreateUser("", "", "", "", "", false, null, out status));
        }

        [Test]
        public void Creating_a_new_user_should_relay_action_to_business_layer()
        {
            Assert.Ignore("HttpContext.Current faken.");

            var email = "john.doe@google.com";
            var password = "Nlwä2$_n32#@";
            var firstName = "John";
            var lastName = "Doe";
            var isApproved = false;

            var status = (MembershipCreateStatus) (-1);
            MembershipUser createdUser = null;
            var creationDate = DateTime.Now;

            With.Mocks(MockFactory).Expecting(delegate
                                                  {
                                                      Expect.Call(MockUserService.CreateUser(null, email, password, firstName, lastName))
                                                          .Return(new UserDto
                                                                      {
                                                                          Email = email,
                                                                          IsApproved = isApproved,
                                                                          CreateDate = creationDate
                                                                      });
                                                  }).Verify(
                                                      delegate
                                                          {
                                                              createdUser = Sut.CreateUser(email, password, firstName, lastName, out status);
                                                          });

            Assert.That(status, Is.EqualTo(MembershipCreateStatus.Success));

            Assert.That(createdUser, Is.Not.Null);
            Assert.That(createdUser.UserName, Is.EqualTo(email));
            Assert.That(createdUser.Email, Is.EqualTo(email));
            Assert.That(createdUser.CreationDate, Is.EqualTo(creationDate));
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