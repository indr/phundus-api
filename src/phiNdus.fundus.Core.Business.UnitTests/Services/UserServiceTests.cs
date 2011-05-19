using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Services
{
    [TestFixture]
    internal class UserServiceTests
    {
        #region SetUp/TearDown

        [SetUp]
        public void SetUp()
        {
            IoC.Initialize(new WindsorContainer());
            MockFactory = new MockRepository();

            MockUnitOfWork = MockFactory.StrictMock<IUnitOfWork>();
            UnitOfWork.RegisterGlobalUnitOfWork(MockUnitOfWork);

            MockUserRepository = MockFactory.StrictMock<IUserRepository>();
            IoC.Container.Register(Component.For<IUserRepository>().Instance(MockUserRepository));

            MockMailGateway = MockFactory.StrictMock<IMailGateway>();
            IoC.Container.Register(Component.For<IMailGateway>().Instance(MockMailGateway));

            Sut = new UserService();

            TedMosby = new User();
            TedMosby.FirstName = "Ted";
            TedMosby.LastName = "Mosby";
            TedMosby.Membership.Email = "ted.mosby@example.com";
            // TODO,Inder: Password encryption
            TedMosby.Membership.Password = "1234";
        }

        #endregion

        private MockRepository MockFactory { get; set; }
        
        private IUnitOfWork MockUnitOfWork { get; set; }
        private IUserRepository MockUserRepository { get; set; }
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
                    MockUserRepository.Save(null)).IgnoreArguments().Return(null);
                Expect.Call(() => MockMailGateway.Send(null, null, null)).IgnoreArguments();
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }
            using (MockFactory.Playback())
            {
                UserDto dto = Sut.CreateUser("Ted.Mosby@example.com", "");

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
                    MockUserRepository.Save(null)).IgnoreArguments().Return(null);
                Expect.Call(() => MockMailGateway.Send(null, null, null)).IgnoreArguments();
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }
            using (MockFactory.Playback())
            {
                UserDto dto = Sut.CreateUser("ted.mosby@example.com", "");

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
        [ExpectedException(typeof (EmailAlreadyTakenException))]
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
                Sut.CreateUser("ted.mosby@example.com", "");
            }
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
        public void ValidateUser_with_unknown_email_returns_false()
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
                Assert.That(actual, Is.False);
            }
        }

        [Test]
        public void ValidateUser_returns_true()
        {
            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("unknown@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                var actual = Sut.ValidateUser("unknown@example.com", "1234");
                Assert.That(actual, Is.True);
            }
        }

        [Test]
        public void ValidateUser_with_invalid_password_returns_false()
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
                Assert.That(actual, Is.False);
            }
        }

        [Test]
        public void ValidateUser_with_uppercase_email_returns_true()
        {
            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("unknown@example.com")).Return(TedMosby);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                var actual = Sut.ValidateUser("UNKNOWN@example.com", "1234");
                Assert.That(actual, Is.True);
            }
        }

        #region UpdateUser
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateUser_with_null_subject_throws()
        {
            Sut.UpdateUser(null);
        }

        [Test]
        [ExpectedException(typeof(EntityNotFoundException))]
        public void UpdateUser_with_invalid_id_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.Get(0)).Return(null);
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                Sut.UpdateUser(new UserDto { Id = 0 });
            }
        }

        [Test]
        [ExpectedException(typeof (DtoOutOfDateException))]
        public void UpdateUser_with_out_of_date_userdto_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.Get(1)).Return(new User());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }

            using (MockFactory.Playback())
            {
                Sut.UpdateUser(new UserDto { Id = 1, Version = -1 });
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
        #endregion
    }
}