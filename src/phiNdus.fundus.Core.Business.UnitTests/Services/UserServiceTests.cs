using System;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using phiNdus.fundus.Core.Domain.Settings;
using phiNdus.fundus.TestHelpers.UnitTests.Settings;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Services
{
    [TestFixture]
    public class UserServiceTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            MockUnitOfWork = CreateAndRegisterStrictUnitOfWorkMock();

            Sut = new UserService();

            TedMosby = new User();
            TedMosby.FirstName = "Ted";
            TedMosby.LastName = "Mosby";
            TedMosby.Membership.Email = "ted.mosby@example.com";
            TedMosby.Membership.IsApproved = true;
            TedMosby.Membership.Password = "1234";
        }

        #endregion

        protected ICommonSettings StubCommonSettings { get; set; }

        protected IMailTemplateSettings StubMailTemplateSettings { get; set; }

        protected ISettings StubSettings { get; set; }

        private IUserRepository MockUserRepository { get; set; }
        private IRoleRepository MockRoleRepository { get; set; }
        private IMailGateway MockMailGateway { get; set; }

        private UserService Sut { get; set; }
        private User TedMosby { get; set; }

       

        [Test]
        public void DeleteUser_deletes_repository_and_flushes_transaction()
        {
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = CreateAndRegisterStrictMock<IMailGateway>();

            var user = new User();
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindByEmail("user@example.com")).Return(user);
                Expect.Call(() => MockUserRepository.Delete(user));
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose()).Repeat.Any();
            }

            using (MockFactory.Playback())
            {
                bool actual = Sut.DeleteUser("user@example.com");
                Assert.That(actual, Is.True);
            }
        }

        [Test]
        public void DeleteUser_with_email_null_throws()
        {
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = CreateAndRegisterStrictMock<IMailGateway>();

            var ex = Assert.Throws<ArgumentNullException>(() => Sut.DeleteUser(null));
            Assert.That(ex.ParamName, Is.EqualTo("email"));
        }

        [Test]
        public void GetUser_lowers_email()
        {
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = CreateAndRegisterStrictMock<IMailGateway>();

            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("ted.mosby@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                UserDto dto = Sut.GetUser("Ted.Mosby@example.com");
                Assert.That(dto, Is.Not.Null);
                Assert.That(dto.Email, Is.EqualTo("ted.mosby@example.com"));
            }
        }

        [Test]
        public void GetUser_returns_dto()
        {
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = CreateAndRegisterStrictMock<IMailGateway>();

            var user = new User();
            user.Membership.Email = "user@example.com";
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindByEmail("user@example.com")).Return(user);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                UserDto actual = Sut.GetUser("user@example.com");
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.Email, Is.EqualTo("user@example.com"));
            }
        }

        [Test]
        public void GetUser_returns_null()
        {
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = CreateAndRegisterStrictMock<IMailGateway>();

            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindByEmail("user@example.com")).Return(null);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                UserDto actual = Sut.GetUser("user@example.com");
                Assert.That(actual, Is.Null);
            }
        }

        [Test]
        public void UpdateUser_updates_repository_and_flushes_transaction()
        {
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = CreateAndRegisterStrictMock<IMailGateway>();

            var user = new User();
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.Get(1)).Return(user);
                Expect.Call(() => MockUserRepository.Update(user));
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                Sut.UpdateUser(new UserDto {Id = 1});
            }
        }

        [Test]
        public void UpdateUser_with_invalid_id_throws()
        {
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = CreateAndRegisterStrictMock<IMailGateway>();

            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.Get(0)).Return(null);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                Assert.Throws<EntityNotFoundException>(() => Sut.UpdateUser(new UserDto {Id = 0}));
            }
        }

        [Test]
        public void UpdateUser_with_null_subject_throws()
        {
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = CreateAndRegisterStrictMock<IMailGateway>();

            Assert.Throws<ArgumentNullException>(() => Sut.UpdateUser(null));
        }

        [Test]
        public void UpdateUser_with_out_of_date_userdto_throws()
        {
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = CreateAndRegisterStrictMock<IMailGateway>();

            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.Get(1)).Return(new User());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                Assert.Throws<DtoOutOfDateException>(() => Sut.UpdateUser(new UserDto {Id = 1, Version = -1}));
            }
        }

        [Test]
        public void ValidateUser_returns_not_null()
        {
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = CreateAndRegisterStrictMock<IMailGateway>();

            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("unknown@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                string actual = Sut.ValidateUser("unknown@example.com", "1234");
                Assert.That(actual, Is.Not.Null);
            }
        }

        [Test]
        public void ValidateUser_with_invalid_password_returns_null()
        {
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = CreateAndRegisterStrictMock<IMailGateway>();

            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("unknown@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                string actual = Sut.ValidateUser("unknown@example.com", "123");
                Assert.That(actual, Is.Null);
            }
        }

        [Test]
        public void ValidateUser_with_unknown_email_returns_null()
        {
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = CreateAndRegisterStrictMock<IMailGateway>();

            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("unknown@example.com")).Return(null);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                string actual = Sut.ValidateUser("unknown@example.com", "");
                Assert.That(actual, Is.Null);
            }
        }

        [Test]
        public void ValidateUser_with_uppercase_email_returns_not_null()
        {
            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = CreateAndRegisterStrictMock<IMailGateway>();

            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("unknown@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                string actual = Sut.ValidateUser("UNKNOWN@example.com", "1234");
                Assert.That(actual, Is.Not.Null);
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