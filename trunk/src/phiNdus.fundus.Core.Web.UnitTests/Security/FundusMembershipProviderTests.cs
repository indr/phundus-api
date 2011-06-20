using System;
using System.Collections.Specialized;
using System.Web.Security;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Web.Security;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Web.UnitTests.Security
{
    [TestFixture]
    public class FundusMembershipProviderTests : MockTestBase<FundusMembershipProvider>
    {
        private IUserService UserServiceMock { get; set; }

        protected override void RegisterDependencies(IWindsorContainer container)
        {
            base.RegisterDependencies(container);

            UserServiceMock = MockFactory.StrictMock<IUserService>();

            container.Register(Component.For<IUserService>().Instance(UserServiceMock));
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
            string email = "john.doe@google.com";
            string oldPassword = "23ioN09*c$sE";
            string newPassword = "Nlwä2$_n32#@";
            bool passwordChanged = false;

            With.Mocks(MockFactory).Expecting(delegate
                                                  {
                                                      Expect.Call(UserServiceMock.ChangePassword(null, email,
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
        public void Creating_a_new_user_should_relay_action_to_business_layer()
        {
            string username = "john.doe@google.com";
            string password = "Nlwä2$_n32#@";
            string passwordQuestion = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr?";
            string passwordAnswer = "sed diam nonumy";
            bool isApproved = false;

            var status = (MembershipCreateStatus) (-1);
            MembershipUser createdUser = null;
            DateTime creationDate = DateTime.Now;

            With.Mocks(MockFactory).Expecting(delegate
                                                  {
                                                      Expect.Call(UserServiceMock.CreateUser(null, username, password))
                                                          .Return(new UserDto
                                                                      {
                                                                          Email = username,
                                                                          IsApproved = isApproved,
                                                                          CreateDate = creationDate
                                                                      });
                                                  }).Verify(
                                                      delegate
                                                          {
                                                              createdUser = Sut.CreateUser(username, password, null,
                                                                                           passwordQuestion,
                                                                                           passwordAnswer, isApproved,
                                                                                           null, out status);
                                                          });

            Assert.That(status, Is.EqualTo(MembershipCreateStatus.Success));

            Assert.That(createdUser, Is.Not.Null);
            Assert.That(createdUser.UserName, Is.EqualTo(username));
            Assert.That(createdUser.Email, Is.EqualTo(username));
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