using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests
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

            Sut = new UserService();
        }

        #endregion

        private IUserService Sut { get; set; }

        private MockRepository MockFactory { get; set; }
        private IUnitOfWork MockUnitOfWork { get; set; }
        private IUserRepository MockUserRepository { get; set; }

        [Test]
        public void CreateUser_lowers_email()
        {
            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("ted.mosby@example.com")).Return(null);
                Expect.Call(
                    MockUserRepository.Save(null)).IgnoreArguments().Return(null);
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }
            using (MockFactory.Playback())
            {
                UserDto dto = Sut.CreateUser("Ted.Mosby@example.com", "", "", "");

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
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }
            using (MockFactory.Playback())
            {
                UserDto dto = Sut.CreateUser("ted.mosby@example.com", "", "", "");

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
                Expect.Call(() => MockUnitOfWork.TransactionalFlush());
                Expect.Call(() => MockUnitOfWork.Dispose());
            }
            using (MockFactory.Playback())
            {
                Sut.CreateUser("ted.mosby@example.com", "", "", "");
            }
        }

        [Test]
        [ExpectedException(typeof (EmailAlreadyTakenException))]
        public void CreateUser_with_email_already_taken_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(
                    MockUserRepository.FindByEmail("ted.mosby@example.com")).Return(new User(0));
                Expect.Call(() => MockUnitOfWork.Dispose());
            }
            using (MockFactory.Playback())
            {
                Sut.CreateUser("ted.mosby@example.com", "", "", "");
            }
        }
    }
}