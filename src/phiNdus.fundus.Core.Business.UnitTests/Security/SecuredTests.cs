using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Security;
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

        private class DoSomethingReturningIntCalled: Exception
        {
            
        }

        private class DummyService
        {
            public void DoSomething()
            {
                throw new DoSomethingCalled();
            }

            public int DoSomethingReturningInt()
            {
                return 1;
            }
        }

        private IUserRepository MockUserRepository { get; set; }
        private User User { get; set; }


        [Test]
        [ExpectedException(typeof (DoSomethingCalled))]
        public void With_session_calls_proc()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("ABCD")).Return(User);
            }

            using (MockFactory.Playback())
            {
                Secured.With(Session.FromKey("ABCD")).Call<DummyService>(s => s.DoSomething());
            }
        }

        [Test]
        public void With_session_calls_func()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.FindBySessionKey("ABCD")).Return(User);
            }

            using (MockFactory.Playback())
            {
                var result = Secured.With(Session.FromKey("ABCD")).Call<DummyService, int>(s => s.DoSomethingReturningInt());
                Assert.That(result, Is.EqualTo(1));
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