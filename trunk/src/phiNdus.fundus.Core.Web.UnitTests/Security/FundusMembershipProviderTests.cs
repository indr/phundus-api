
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Web.Security;
using phiNdus.fundus.Core.Business;
using Rhino.Mocks;
using System.Web.Security;

namespace phiNdus.fundus.Core.Web.UnitTests.Security {

    [TestFixture]
    public class FundusMembershipProviderTests {

        private FundusMembershipProvider Sut { get; set; }

        private IUserService UserServiceMock { get; set; }

        private MockRepository MockFactory { get; set; }

        [SetUp]
        public void Setup() {
            this.MockFactory = new MockRepository();

            this.UserServiceMock = this.MockFactory.StrictMock<IUserService>();

            this.Sut = new FundusMembershipProvider(this.UserServiceMock);
        }

        [Test]
        public void Ensure_password_reset_is_enabled() {
            Assert.IsTrue(this.Sut.EnablePasswordReset);
        }

        [Test]
        public void Ensure_password_retrieval_is_disabled() {
            Assert.IsFalse(this.Sut.EnablePasswordRetrieval);
        }

        [Test]
        public void Changing_the_password_should_relay_action_to_business_layer() {
            var email = "john.doe@google.com";
            var oldPassword = "23ioN09*c$sE";
            var newPassword = "Nlwä2$_n32#@";
            var passwordChanged = false;

            With.Mocks(this.MockFactory).Expecting(delegate {
                Expect.Call(this.UserServiceMock.ChangePassword(email, oldPassword, newPassword))
                    .Return(true);
            }).Verify(delegate {
                passwordChanged = this.Sut.ChangePassword(email, oldPassword, newPassword);
            });

            Assert.IsTrue(passwordChanged);
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
                Expect.Call(this.UserServiceMock.CreateUser(username, password, passwordQuestion, passwordAnswer, isApproved))
                    .Return(new UserDto {
                        Email = username,
                        IsApproved = isApproved,
                        CreateDate =creationDate,
                        PasswordQuestion = passwordQuestion
                    });
            }).Verify(delegate {
                createdUser = this.Sut.CreateUser(username, password, null, passwordQuestion, passwordAnswer, isApproved, null, out status);
            });

            Assert.AreEqual(MembershipCreateStatus.Success, status);

            Assert.IsNotNull(createdUser);
            Assert.AreEqual(username, createdUser.UserName);
            Assert.AreEqual(username, createdUser.Email);
            Assert.AreEqual(isApproved, createdUser.IsApproved);
            Assert.AreEqual(passwordQuestion, createdUser.PasswordQuestion);
            Assert.AreEqual(creationDate, createdUser.CreationDate);
        }
    }
}
