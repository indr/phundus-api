using System;
using NUnit.Framework;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.SecuredServices;
using phiNdus.fundus.Business.Security;
using phiNdus.fundus.Business.Services;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Business.UnitTests.SecuredServices
{
    [TestFixture]
    public class SecuredUserServiceTests : UnitTestBase<SecuredUserService>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            MockUnitOfWork = Obsolete_CreateAndRegisterDynamicUnitOfWorkMock();
            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
            MockUserService = Obsolete_CreateAndRegisterStrictMock<UserService>();

            User = new User(1);
            User.Role = Role.User;
            User.Membership.Email = "user@example.com";

            Admin = new User(1);
            Admin.Role = Role.Administrator;
            Admin.Membership.Email = "admin@example.com";

            Sut = new SecuredUserService();
        }

        #endregion

        protected UserService MockUserService { get; set; }
        protected IUserRepository MockUserRepository { get; set; }
        protected User User { get; set; }
        protected User Admin { get; set; }

        [Test]
        public void CreateUser_with_sessionKey_null_does_not_throw()
        {
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserService.CreateUser("user@example.com", "1234", "", "", 1, null));
            }
            using (Obsolete_MockFactory.Playback())
            {
                Sut.CreateUser(null, "user@example.com", "1234", "", "", 1, null);
            }
        }

        [Test]
        public void CreateUser_returns_dto()
        {
            var dto = new UserDto();
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserService.CreateUser("user@example.com", "1234", "John", "Doe", 1, null)).Return(dto);
            }
            using (Obsolete_MockFactory.Playback())
            {
                var actual = Sut.CreateUser("maybevalid", "user@example.com", "1234", "John", "Doe", 1, null);
                Assert.That(actual, Is.SameAs(dto));
            }
        }

        [Test]
        public void DeleteUser_other_with_administrator_roll()
        {
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("valid")).Return(Admin);
                Expect.Call(MockUserService.DeleteUser("user@example.com")).Return(true);
            }
            using (Obsolete_MockFactory.Playback())
            {
                var actual = Sut.DeleteUser("valid", "user@example.com");
                Assert.That(actual, Is.True);
            }
        }

        [Test]
        public void DeleteUser_other_with_user_roll_throws()
        {
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("valid")).Return(User);
            }
            using (Obsolete_MockFactory.Playback())
            {
                Assert.Throws<AuthorizationException>(() => Sut.DeleteUser("valid", "other@example.com"));
            }
        }

        [Test]
        public void DeleteUser_with_invalid_sessionKey_throws()
        {
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("invalid")).Return(null);
            }
            using (Obsolete_MockFactory.Playback())
            {
                Assert.Throws<InvalidSessionKeyException>(() => Sut.DeleteUser("invalid", null));
            }
        }

        [Test]
        public void DeleteUser_with_sessionKey_null_throws()
        {
            Obsolete_MockFactory.ReplayAll();
            using (Obsolete_MockFactory.Playback())
            {
                var ex = Assert.Throws<ArgumentNullException>(() => Sut.DeleteUser(null, ""));
                Assert.That(ex.ParamName, Is.EqualTo("key"));
            }
        }

        [Test]
        public void GetUser_other_with_user_roll_throws()
        {
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("valid")).Return(User);
            }
            using (Obsolete_MockFactory.Playback())
            {
                Assert.Throws<AuthorizationException>(() => Sut.GetUser("valid", "other@example.com"));
            }
        }

        [Test]
        public void GetUser_own_with_user_roll()
        {
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("valid")).Return(User);
                Expect.Call(MockUserService.GetUser("user@example.com")).Return(new UserDto());
            }
            using (Obsolete_MockFactory.Playback())
            {
                var actual = Sut.GetUser("valid", "user@example.com");
                Assert.That(actual, Is.Not.Null);
            }
        }

        [Test]
        public void GetUser_with_invalid_sessionKey_throws()
        {
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("invalid")).Return(null);
            }
            using (Obsolete_MockFactory.Playback())
            {
                Assert.Throws<InvalidSessionKeyException>(() => Sut.GetUser("invalid", ""));
            }
        }

        [Test]
        public void GetUser_with_sessionKey_null_throws()
        {
            Obsolete_MockFactory.ReplayAll();
            using (Obsolete_MockFactory.Playback())
            {
                var ex = Assert.Throws<ArgumentNullException>(() => Sut.GetUser(null, ""));
                Assert.That(ex.ParamName, Is.EqualTo("key"));
            }
        }

        [Test]
        public void UpdateUser_with_user_null_throws()
        {
            Obsolete_MockFactory.ReplayAll();
            using (Obsolete_MockFactory.Playback())
            {
                var ex = Assert.Throws<ArgumentNullException>(() => Sut.UpdateUser("valid", null));
                Assert.That(ex.ParamName, Is.EqualTo("user"));
            }
        }

        [Test]
        public void UpdateUser_with_invalid_sessionKey_throws()
        {
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("invalid")).Return(null);
            }
            using (Obsolete_MockFactory.Playback())
            {
                Assert.Throws<InvalidSessionKeyException>(() => Sut.UpdateUser("invalid", new UserDto()));
            }
        }

        [Test]
        public void UpdateUser_with_sessionKey_null_throws()
        {
            Obsolete_MockFactory.ReplayAll();
            using (Obsolete_MockFactory.Playback())
            {
                var ex = Assert.Throws<ArgumentNullException>(() => Sut.UpdateUser(null, new UserDto()));
                Assert.That(ex.ParamName, Is.EqualTo("key"));
            }
        }

        [Test]
        public void UpdateUser_other_with_user_roll_throws()
        {
            var userDto = new UserDto();
            userDto.Id = 2;

            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("valid")).Return(User);
            }
            using (Obsolete_MockFactory.Playback())
            {
                Assert.Throws<AuthorizationException>(() => Sut.UpdateUser("valid", userDto));
            }
        }

        [Test]
        public void UpdateUser_other_with_administrator_roll()
        {
            var userDto = new UserDto();
            userDto.Id = 2;

            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("valid")).Return(Admin);
                Expect.Call(() => MockUserService.UpdateUser(userDto));
            }
            using (Obsolete_MockFactory.Playback())
            {
                Sut.UpdateUser("valid", userDto);
            }
        }

        private string GetNewSessionId()
        {
            return Guid.NewGuid().ToString("N");
        }

        [Test]
        public void ValidateUser_returns()
        {
            var sessionId = GetNewSessionId();
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserService.ValidateUser(Arg<string>.Is.Equal(sessionId),
                     Arg<string>.Is.Equal("user@example.com"),
                     Arg<string>.Is.Equal("1234"))).Return(true);
            }
            using (Obsolete_MockFactory.Playback())
            {
                var actual = Sut.ValidateUser(sessionId, "user@example.com", "1234");
                Assert.That(actual, Is.True);
            }
        }
    }
}