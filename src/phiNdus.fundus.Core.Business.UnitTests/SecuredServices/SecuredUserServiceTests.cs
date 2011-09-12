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
    public class SecuredUserServiceTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            MockUnitOfWork = CreateAndRegisterDynamicUnitOfWorkMock();
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockUserService = CreateAndRegisterStrictMock<UserService>();

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
        protected SecuredUserService Sut { get; set; }
        protected User User { get; set; }
        protected User Admin { get; set; }

        [Test]
        public void CreateUser_with_sessionKey_null_does_not_throw()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserService.CreateUser("user@example.com", "1234", "", ""));
            }
            using (MockFactory.Playback())
            {
                Sut.CreateUser(null, "user@example.com", "1234", "", "");
            }
        }

        [Test]
        public void CreateUser_returns_dto()
        {
            var dto = new UserDto();
            using (MockFactory.Record())
            {
                Expect.Call(MockUserService.CreateUser("user@example.com", "1234", "John", "Doe")).Return(dto);
            }
            using (MockFactory.Playback())
            {
                var actual = Sut.CreateUser("maybevalid", "user@example.com", "1234", "John", "Doe");
                Assert.That(actual, Is.SameAs(dto));
            }
        }

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
        public void UpdateUser_with_user_null_throws()
        {
            MockFactory.ReplayAll();
            using (MockFactory.Playback())
            {
                var ex = Assert.Throws<ArgumentNullException>(() => Sut.UpdateUser("valid", null));
                Assert.That(ex.ParamName, Is.EqualTo("user"));
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
                Assert.Throws<InvalidSessionKeyException>(() => Sut.UpdateUser("invalid", new UserDto()));
            }
        }

        [Test]
        public void UpdateUser_with_sessionKey_null_throws()
        {
            MockFactory.ReplayAll();
            using (MockFactory.Playback())
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

            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("valid")).Return(User);
            }
            using (MockFactory.Playback())
            {
                Assert.Throws<AuthorizationException>(() => Sut.UpdateUser("valid", userDto));
            }
        }

        [Test]
        public void UpdateUser_other_with_administrator_roll()
        {
            var userDto = new UserDto();
            userDto.Id = 2;

            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("valid")).Return(Admin);
                Expect.Call(() => MockUserService.UpdateUser(userDto));
            }
            using (MockFactory.Playback())
            {
                Sut.UpdateUser("valid", userDto);
            }
        }

        private string GetNewSessionId()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        [Test]
        public void ValidateUser_returns()
        {
            var sessionId = GetNewSessionId();
            using (MockFactory.Record())
            {
                Expect.Call(MockUserService.ValidateUser(Arg<string>.Is.Equal(sessionId),
                     Arg<string>.Is.Equal("user@example.com"),
                     Arg<string>.Is.Equal("1234"))).Return(true);
            }
            using (MockFactory.Playback())
            {
                var actual = Sut.ValidateUser(sessionId, "user@example.com", "1234");
                Assert.That(actual, Is.True);
            }
        }
    }
}