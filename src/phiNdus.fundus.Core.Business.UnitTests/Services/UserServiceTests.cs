using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
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

            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            MockRoleRepository = CreateAndRegisterStrictMock<IRoleRepository>();
            MockMailGateway = CreateAndRegisterStrictMock<IMailGateway>();

            Sut = new UserService();

            TedMosby = new User();
            TedMosby.FirstName = "Ted";
            TedMosby.LastName = "Mosby";
            TedMosby.Membership.Email = "ted.mosby@example.com";
            TedMosby.Membership.IsApproved = true;
            TedMosby.Membership.Password = "1234";
        }

        #endregion

        private IUserRepository MockUserRepository { get; set; }
        private IRoleRepository MockRoleRepository { get; set; }
        private IMailGateway MockMailGateway { get; set; }

        private UserService Sut { get; set; }
        private User TedMosby { get; set; }

        [Test]
        public void CreateUser_lowers_email()
        {
            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("ted.mosby@example.com")).Return(null);
                Expect.Call(
                    MockRoleRepository.Get(1)).Return(Role.User);
                Expect.Call(
                    MockUserRepository.Save(null)).IgnoreArguments().Return(null);

                Expect.Call(() => MockMailGateway.Send(null, null, null)).IgnoreArguments();
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }
            using (MockFactory.Playback())
            {
                var dto = Sut.CreateUser("Ted.Mosby@example.com", "");

                Assert.That(dto, Is.Not.Null);
                Assert.That(dto.Email, Is.EqualTo("ted.mosby@example.com"));
            }
        }

        [Test]
        public void CreateUser_returns_dto()
        {
            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("ted.mosby@example.com")).Return(null);
                Expect.Call(
                    MockRoleRepository.Get(1)).Return(Role.User);
                Expect.Call(
                    MockUserRepository.Save(null)).IgnoreArguments().Return(null);
                Expect.Call(() => MockMailGateway.Send(null, null, null)).IgnoreArguments();
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }
            using (MockFactory.Playback())
            {
                var dto = Sut.CreateUser("ted.mosby@example.com", "");

                Assert.That(dto, Is.Not.Null);
                Assert.That(dto.Email, Is.EqualTo("ted.mosby@example.com"));
            }
        }

        [Test]
        public void CreateUser_saves_new_user_to_repository()
        {
            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("ted.mosby@example.com")).Return(null);
                Expect.Call(
                    MockRoleRepository.Get(1)).Return(Role.User);
                Expect.Call(
                    MockUserRepository.Save(null)).IgnoreArguments().Return(null);
                Expect.Call(() => MockMailGateway.Send(null, null, null)).IgnoreArguments();
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }
            using (MockFactory.Playback())
            {
                Sut.CreateUser("ted.mosby@example.com", "");
            }
        }

        [Test]
        public void CreateUser_with_email_already_taken_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("ted.mosby@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }
            using (MockFactory.Playback())
            {
                Assert.Throws<EmailAlreadyTakenException>(() => Sut.CreateUser("ted.mosby@example.com", ""));
            }
        }

        [Test]
        public void DeleteUser_deletes_repository_and_flushes_transaction()
        {
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
                var actual = Sut.DeleteUser("user@example.com");
                Assert.That(actual, Is.True);
            }
        }

        [Test]
        public void DeleteUser_with_email_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Sut.DeleteUser(null));
            Assert.That(ex.ParamName, Is.EqualTo("email"));
        }

        [Test]
        public void GetUser_lowers_email()
        {
            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("ted.mosby@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                var dto = Sut.GetUser("Ted.Mosby@example.com");
                Assert.That(dto, Is.Not.Null);
                Assert.That(dto.Email, Is.EqualTo("ted.mosby@example.com"));
            }
        }

        [Test]
        public void GetUser_returns_dto()
        {
            var user = new User();
            user.Membership.Email = "user@example.com";
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindByEmail("user@example.com")).Return(user);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                var actual = Sut.GetUser("user@example.com");
                Assert.That(actual, Is.Not.Null);
                Assert.That(actual.Email, Is.EqualTo("user@example.com"));
            }
        }

        [Test]
        public void GetUser_returns_null()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindByEmail("user@example.com")).Return(null);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                var actual = Sut.GetUser("user@example.com");
                Assert.That(actual, Is.Null);
            }
        }

        [Test]
        public void UpdateUser_updates_repository_and_flushes_transaction()
        {
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
            Assert.Throws<ArgumentNullException>(() => Sut.UpdateUser(null));
        }

        [Test]
        public void UpdateUser_with_out_of_date_userdto_throws()
        {
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
            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("unknown@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                var actual = Sut.ValidateUser("unknown@example.com", "1234");
                Assert.That(actual, Is.Not.Null);
            }
        }

        [Test]
        public void ValidateUser_with_invalid_password_returns_null()
        {
            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("unknown@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                var actual = Sut.ValidateUser("unknown@example.com", "123");
                Assert.That(actual, Is.Null);
            }
        }

        [Test]
        public void ValidateUser_with_unknown_email_returns_null()
        {
            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("unknown@example.com")).Return(null);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                var actual = Sut.ValidateUser("unknown@example.com", "");
                Assert.That(actual, Is.Null);
            }
        }

        [Test]
        public void ValidateUser_with_uppercase_email_returns_not_null()
        {
            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("unknown@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                var actual = Sut.ValidateUser("UNKNOWN@example.com", "1234");
                Assert.That(actual, Is.Not.Null);
            }
        }
    }
}