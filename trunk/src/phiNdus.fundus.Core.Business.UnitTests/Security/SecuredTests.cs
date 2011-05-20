using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Security
{
    [TestFixture]
    internal class SecuredTests : BaseTestFixture
    {
        #region Setup/Teardown

        public override void SetUp()
        {
            base.SetUp();

            MockUserRepository = CreateAndRegisterStrictMock<IUserRepository>();
            User = new User();
        }

        #endregion

        private class DoSomethingCalled : Exception
        {
        }

        private class DummyService : BaseService
        {
// ReSharper disable MemberCanBeMadeStatic.Local
            public void DoSomethingThrowingException()
// ReSharper restore MemberCanBeMadeStatic.Local
            {
                throw new DoSomethingCalled();
            }

// ReSharper disable MemberCanBeMadeStatic.Local
            public int DoSomethingReturningInt()
// ReSharper restore MemberCanBeMadeStatic.Local
            {
                return 1;
            }
        }

        private IUserRepository MockUserRepository { get; set; }
        private User User { get; set; }


        [Test]
        public void With_session_calls_func()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("ABCD")).Return(User);
            }

            using (MockFactory.Playback())
            {
                int result =
                    Secured.With(Session.FromKey("ABCD")).Call<DummyService, int>(
                        service => service.DoSomethingReturningInt());
                Assert.That(result, Is.EqualTo(1));
            }
        }

        [Test]
        [ExpectedException(typeof (DoSomethingCalled))]
        public void With_session_calls_action()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("ABCD")).Return(User);
            }

            using (MockFactory.Playback())
            {
                Secured.With(Session.FromKey("ABCD")).Call<DummyService>(
                    service => service.DoSomethingThrowingException());
            }
        }


        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void With_session_null_throws()
        {
            Secured.With(null);
        }

        /*
        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void WithSession_with_sessionKey_null_throws()
        {
            Secured.WithSession(null);
        }

        [Test]
        [ExpectedException(typeof (InvalidSessionKeyException))]
        public void WithSession_with_invalid_sessionKey_throws()
        {
            Secured.WithSession("");
        }
         * */
    }
}