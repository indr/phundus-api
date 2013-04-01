using System;
using NUnit.Framework;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Business.Gateways;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using phiNdus.fundus.Domain.Settings;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Mocks;

namespace phiNdus.fundus.Business.UnitTests.ServicesTests
{
    [TestFixture]
    public class UserServiceTests : UnitTestBase<Business.Services.UserService>
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {

            base.SetUp();

            MockUnitOfWork = Obsolete_CreateAndRegisterStrictUnitOfWorkMock();

            Sut = new Business.Services.UserService();

            TedMosby = new User();
            TedMosby.FirstName = "Ted";
            TedMosby.LastName = "Mosby";
            TedMosby.Membership.Email = "ted.mosby@example.com";
            TedMosby.Membership.IsApproved = true;
            TedMosby.Membership.Password = "1234";
        }

        #endregion

        protected IMailTemplateSettings StubMailTemplateSettings { get; set; }

        protected ISettings StubSettings { get; set; }

        private IUserRepository MockUserRepository { get; set; }
        private IRoleRepository MockRoleRepository { get; set; }
        private IMailGateway MockMailGateway { get; set; }

        private User TedMosby { get; set; }

       

        [Test]
        public void DeleteUser_deletes_repository_and_flushes_transaction()
        {
            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = Obsolete_CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = Obsolete_CreateAndRegisterStrictMock<IMailGateway>();

            var user = new User();
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindByEmail("user@example.com")).Return(user);
                Expect.Call(() => MockUserRepository.Delete(user));
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose()).Repeat.Any();
            }

            using (Obsolete_MockFactory.Playback())
            {
                bool actual = Sut.DeleteUser("user@example.com");
                Assert.That(actual, Is.True);
            }
        }

        [Test]
        public void DeleteUser_with_email_null_throws()
        {
            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = Obsolete_CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = Obsolete_CreateAndRegisterStrictMock<IMailGateway>();

            var ex = Assert.Throws<ArgumentNullException>(() => Sut.DeleteUser(null));
            Assert.That(ex.ParamName, Is.EqualTo("email"));
        }

        [Test]
        public void GetUser_lowers_email()
        {
            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = Obsolete_CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = Obsolete_CreateAndRegisterStrictMock<IMailGateway>();

            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("ted.mosby@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (Obsolete_MockFactory.Playback())
            {
                UserDto dto = Sut.GetUser("Ted.Mosby@example.com");
                Assert.That(dto, Is.Not.Null);
                Assert.That(dto.Email, Is.EqualTo("ted.mosby@example.com"));
            }
        }

        [Test]
        public void GetUser_returns_dto()
        {
            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = Obsolete_CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = Obsolete_CreateAndRegisterStrictMock<IMailGateway>();

            var user = new User();
            user.Membership.Email = "user@example.com";
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindByEmail("user@example.com")).Return(user);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (Obsolete_MockFactory.Playback())
            {
                UserDto actual = Sut.GetUser("user@example.com");
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.Email, Is.EqualTo("user@example.com"));
            }
        }

        [Test]
        public void GetUser_returns_null()
        {
            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = Obsolete_CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = Obsolete_CreateAndRegisterStrictMock<IMailGateway>();

            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindByEmail("user@example.com")).Return(null);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (Obsolete_MockFactory.Playback())
            {
                UserDto actual = Sut.GetUser("user@example.com");
                Assert.That(actual, Is.Null);
            }
        }

        [Test]
        public void UpdateUser_updates_repository_and_flushes_transaction()
        {
            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = Obsolete_CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = Obsolete_CreateAndRegisterStrictMock<IMailGateway>();

            var user = new User();
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.Get(1)).Return(user);
                Expect.Call(MockRoleRepository.Get(0)).Return(null);
                Expect.Call(() => MockUserRepository.Update(user));
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (Obsolete_MockFactory.Playback())
            {
                Sut.UpdateUser(new UserDto { Id = 1 });
            }
        }

        [Test]
        public void UpdateUser_with_invalid_id_throws()
        {
            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = Obsolete_CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = Obsolete_CreateAndRegisterStrictMock<IMailGateway>();

            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.Get(0)).Return(null);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (Obsolete_MockFactory.Playback())
            {
                Assert.Throws<EntityNotFoundException>(() => Sut.UpdateUser(new UserDto { Id = 0 }));
            }
        }

        [Test]
        public void UpdateUser_with_null_subject_throws()
        {
            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = Obsolete_CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = Obsolete_CreateAndRegisterStrictMock<IMailGateway>();

            Assert.Throws<ArgumentNullException>(() => Sut.UpdateUser(null));
        }

        [Test]
        public void UpdateUser_with_out_of_date_userdto_throws()
        {
            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = Obsolete_CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = Obsolete_CreateAndRegisterStrictMock<IMailGateway>();

            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(MockUserRepository.Get(1)).Return(new User());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (Obsolete_MockFactory.Playback())
            {
                Assert.Throws<DtoOutOfDateException>(() => Sut.UpdateUser(new UserDto {Id = 1, Version = -1}));
            }
        }

        private string GetNewSessionId()
        {
            return Guid.NewGuid().ToString("N");
        }

        [Test]
        public void ValidateUser_returns_true()
        {
            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = Obsolete_CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = Obsolete_CreateAndRegisterStrictMock<IMailGateway>();

            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("unknown@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (Obsolete_MockFactory.Playback())
            {
                bool actual = Sut.ValidateUser(GetNewSessionId(), "unknown@example.com", "1234");
                Assert.That(actual, Is.True);
            }
        }

        [Test]
        public void ValidateUser_with_invalid_password_returns_null()
        {
            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = Obsolete_CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = Obsolete_CreateAndRegisterStrictMock<IMailGateway>();

            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("unknown@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (Obsolete_MockFactory.Playback())
            {
                bool actual = Sut.ValidateUser(GetNewSessionId(), "unknown@example.com", "123");
                Assert.That(actual, Is.False);
            }
        }

        [Test]
        public void ValidateUser_with_unknown_email_returns_null()
        {
            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = Obsolete_CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = Obsolete_CreateAndRegisterStrictMock<IMailGateway>();

            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("unknown@example.com")).Return(null);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (Obsolete_MockFactory.Playback())
            {
                bool actual = Sut.ValidateUser(GetNewSessionId(), "unknown@example.com", "");
                Assert.That(actual, Is.False);
            }
        }

        [Test]
        public void ValidateUser_with_uppercase_email_returns_not_null()
        {
            MockUserRepository = Obsolete_CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = Obsolete_CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = Obsolete_CreateAndRegisterStrictMock<IMailGateway>();

            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("unknown@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (Obsolete_MockFactory.Playback())
            {
                bool actual = Sut.ValidateUser(GetNewSessionId(), "UNKNOWN@example.com", "1234");
                Assert.That(actual, Is.True);
            }
        }
    }

    public static class RhinoExtensions
    {
        /// <summary>
        /// Clears the behavior already recorded in a Rhino Mocks stub.
        /// </summary>
        public static void ClearBehavior<T>(this T stub)
        {
            stub.BackToRecord(BackToRecordOptions.All);
            stub.Replay();
        }
    }
}