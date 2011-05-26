using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.SecuredServices;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.SecuredServices
{
    [TestFixture]
    internal class SecuredUserServiceTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            MockUnitOfWork = CreateAndRegisterDynamicUnitOfWorkMock();
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockUserService = CreateAndRegisterStrictMock<UserService>();

            User = new User();
            User.Role = Role.User;
            User.Membership.Email = "user@example.com";

            Admin = new User();
            Admin.Role = Role.Administrator;
            Admin.Membership.Email = "admin@example.com";

            Sut = new SecuredUserService();
        }

        #endregion

        protected UserService MockUserService { get; set; }
        protected IUserRepository MockUserRepository { get; set; }
        protected SecuredUserService Sut { get; set; }
        protected User User { get; set; }
        protected User Admin { get; set; }

        [Test]
        public void DeleteUser_other_with_administrator_roll()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("valid")).Return(Admin);
                Expect.Call(MockUserService.DeleteUser("user@example.com")).Return(true);
            }
            using (MockFactory.Playback())
            {
                var actual = Sut.DeleteUser("valid", "user@example.com");
                Assert.That(actual, Is.True);
            }
        }

        [Test]
        public void DeleteUser_other_with_user_roll_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("valid")).Return(User);
            }
            using (MockFactory.Playback())
            {
                Assert.Throws<AuthorizationException>(() => Sut.DeleteUser("valid", "other@example.com"));
            }
        }

        [Test]
        public void DeleteUser_with_invalid_sessionKey_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("invalid")).Return(null);
            }
            using (MockFactory.Playback())
            {
                Assert.Throws<InvalidSessionKeyException>(() => Sut.DeleteUser("invalid", null));
            }
        }

        [Test]
        public void DeleteUser_with_sessionKey_null_throws()
        {
            MockFactory.ReplayAll();
            using (MockFactory.Playback())
            {
                var ex = Assert.Throws<ArgumentNullException>(() => Sut.DeleteUser(null, ""));
                Assert.That(ex.ParamName, Is.EqualTo("key"));
            }
        }

        [Test]
        public void GetUser_other_with_user_roll_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("valid")).Return(User);
            }
            using (MockFactory.Playback())
            {
                Assert.Throws<AuthorizationException>(() => Sut.GetUser("valid", "other@example.com"));
            }
        }

        [Test]
        public void GetUser_own_with_user_roll()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("valid")).Return(User);
                Expect.Call(MockUserService.GetUser("user@example.com")).Return(new UserDto());
            }
            using (MockFactory.Playback())
            {
                var actual = Sut.GetUser("valid", "user@example.com");
                Assert.That(actual, Is.Not.Null);
            }
        }

        [Test]
        public void GetUser_with_invalid_sessionKey_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("invalid")).Return(null);
            }
            using (MockFactory.Playback())
            {
                Assert.Throws<InvalidSessionKeyException>(() => Sut.GetUser("invalid", ""));
            }
        }

        [Test]
        public void GetUser_with_sessionKey_null_throws()
        {
            MockFactory.ReplayAll();
            using (MockFactory.Playback())
            {
                var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetUser(null, ""));
                Assert.That(ex.ParamName, Is.EqualTo("key"));
            }
        }

        [Test]
        public void UpdateUser_with_invalid_sessionKey_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("invalid")).Return(null);
            }
            using (MockFactory.Playback())
            {
                Assert.Throws<InvalidSessionKeyException>(() => Sut.UpdateUser("invalid", null));
            }
        }

        [Test]
        public void UpdateUser_with_sessionKey_null_throws()
        {
            MockFactory.ReplayAll();
            using (MockFactory.Playback())
            {
                var ex = Assert.Throws<ArgumentNullException>(() => Sut.UpdateUser(null, null));
                Assert.That(ex.ParamName, Is.EqualTo("key"));
            }
        }

        [Test]
        public void ValidateUser_returns()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserService.ValidateUser("user@example.com", "1234")).Return("valid");
            }
            using (MockFactory.Playback())
            {
                var actual = Sut.ValidateUser("user@example.com", "1234");
                Assert.That(actual, Is.EqualTo("valid"));
            }
        }
    }
}