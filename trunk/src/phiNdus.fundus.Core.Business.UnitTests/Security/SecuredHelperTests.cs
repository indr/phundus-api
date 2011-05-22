using NUnit.Framework;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Business.UnitTests.Security.Constraints;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Security
{
    [TestFixture]
    internal class SecuredHelperTests : BaseTestFixture
    {
        private SecuredHelper Sut { get; set; }

        public override void SetUp()
        {
            base.SetUp();

            using (MockFactory.Record()) {
                Expect.Call(() => MockUnitOfWork.Dispose());
            }
        }

        [Test]
        public void Do_does_not_call_func_lambda_and_throws_when_constraints_are_not_met()
        {
            using (MockFactory.Playback())
            {
                var invoked = false;
                Sut = new SecuredHelper(new AlwaysFalseConstraint());
                Assert.Throws<AuthorizationException>(() => Sut.Do<BaseService, int>(service =>
                                                                                         {
                                                                                             invoked = true;
                                                                                             return 0;
                                                                                         }));
                Assert.That(invoked, Is.False);
            }
        }

        [Test]
        public void Do_does_not_call_proc_lambda_and_throws_when_constraints_are_not_met()
        {
            using (MockFactory.Playback())
            {
                var invoked = false;
                Sut = new SecuredHelper(new AlwaysFalseConstraint());
                Assert.Throws<AuthorizationException>(() => Sut.Do<BaseService>(service => invoked = true));
                Assert.That(invoked, Is.False);
            }
        }


        [Test]
        public void Do_func_instantiates_service()
        {
            using (MockFactory.Playback())
            {
                BaseService serviceRef = null;
                Sut = new SecuredHelper(new AlwaysTrueConstraint());
                Sut.Do<BaseService, int>(service =>
                                             {
                                                 serviceRef = service;
                                                 return 0;
                                             });
                Assert.That(serviceRef, Is.Not.Null);
            }
        }

        [Test]
        public void Do_func_invokes_lambda()
        {
            using (MockFactory.Playback())
            {
                var invoked = false;
                Sut = new SecuredHelper(new AlwaysTrueConstraint());
                Sut.Do<BaseService, int>(service =>
                                             {
                                                 invoked = true;
                                                 return 0;
                                             });
                Assert.That(invoked, Is.True);
            }
        }

        [Test]
        public void Do_func_sets_SecurityContext_on_service()
        {
            using (MockFactory.Playback())
            {
                SecurityContext securityContext = null;
                Sut = new SecuredHelper(new AlwaysTrueConstraint());
                Sut.Do<BaseService, int>(service =>
                                             {
                                                 securityContext = service.SecurityContext;
                                                 return 0;
                                             });
                Assert.That(securityContext, Is.Not.Null);
            }
        }

        [Test]
        public void Do_proc_instantiates_service()
        {
            using (MockFactory.Playback())
            {
                BaseService serviceRef = null;
                Sut = new SecuredHelper(new AlwaysTrueConstraint());
                Sut.Do<BaseService>(service => serviceRef = service);
                Assert.That(serviceRef, Is.Not.Null);
            }
        }

        [Test]
        public void Do_proc_invokes_lambda()
        {
            using (MockFactory.Playback())
            {
                var invoked = false;
                Sut = new SecuredHelper(new AlwaysTrueConstraint());
                Sut.Do<BaseService>(service => { invoked = true; });
                Assert.That(invoked, Is.True);
            }
        }

        [Test]
        public void Do_proc_sets_SecurityContext_on_service()
        {
            using (MockFactory.Playback())
            {
                SecurityContext securityContext = null;
                Sut = new SecuredHelper(new AlwaysTrueConstraint());
                Sut.Do<BaseService>(service => securityContext = service.SecurityContext);
                Assert.That(securityContext, Is.Not.Null);
            }
        }
    }
}