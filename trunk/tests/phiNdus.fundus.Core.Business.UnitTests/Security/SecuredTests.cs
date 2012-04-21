using NUnit.Framework;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Business.UnitTests.Security.Constraints;
using phiNdus.fundus.TestHelpers.TestBases;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Security
{
    [TestFixture]
    public class SecuredTests : UnitTestBase<Secured>
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(() => MockUnitOfWork.Dispose());
            }
        }

        [Test]
        public void Do_does_not_call_func_lambda_and_throws_when_constraints_are_not_met()
        {
            // Arrange
            Sut = Secured.With(new AlwaysFalseConstraint());
            var invoked = false;

            // Act
            var ex = Assert.Throws<AuthorizationException>(() => Sut.Do<BaseService, int>(service =>
                                                                                         {
                                                                                             invoked = true;
                                                                                             return 0;
                                                                                         }));
            // Assert
            Assert.That(invoked, Is.False);
        }

        [Test]
        public void Do_does_not_call_func_lambda_and_throws_when_constraint_added_with_And_is_not_met()
        {
            using (Obsolete_MockFactory.Playback())
            {
                var invoked = false;
                Sut = Secured.With(new AlwaysTrueConstraint());
                Sut.And(new AlwaysFalseConstraint());
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
            using (Obsolete_MockFactory.Playback())
            {
                var invoked = false;
                Sut = Secured.With(new AlwaysFalseConstraint());
                Assert.Throws<AuthorizationException>(() => Sut.Do<BaseService>(service => invoked = true));
                Assert.That(invoked, Is.False);
            }
        }

        [Test]
        public void Do_does_not_call_proc_lambda_and_throws_when_constraint_added_with_And_is_not_met()
        {
            using (Obsolete_MockFactory.Playback())
            {
                var invoked = false;
                Sut = Secured.With(new AlwaysTrueConstraint());
                Sut.And(new AlwaysFalseConstraint());
                Assert.Throws<AuthorizationException>(() => Sut.Do<BaseService>(service => invoked = true));
                Assert.That(invoked, Is.False);
            }
        }

        [Test]
        public void Do_func_instantiates_service()
        {
            using (Obsolete_MockFactory.Playback())
            {
                BaseService serviceRef = null;
                Sut = Secured.With(new AlwaysTrueConstraint());
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
            using (Obsolete_MockFactory.Playback())
            {
                var invoked = false;
                Sut = Secured.With(new AlwaysTrueConstraint());
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
            using (Obsolete_MockFactory.Playback())
            {
                SecurityContext securityContext = null;
                Sut = Secured.With(new AlwaysTrueConstraint());
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
            using (Obsolete_MockFactory.Playback())
            {
                BaseService serviceRef = null;
                Sut = Secured.With(new AlwaysTrueConstraint());
                Sut.Do<BaseService>(service => serviceRef = service);
                Assert.That(serviceRef, Is.Not.Null);
            }
        }

        [Test]
        public void Do_proc_invokes_lambda()
        {
            using (Obsolete_MockFactory.Playback())
            {
                var invoked = false;
                Sut = Secured.With(new AlwaysTrueConstraint());
                Sut.Do<BaseService>(service => { invoked = true; });
                Assert.That(invoked, Is.True);
            }
        }

        [Test]
        public void Do_proc_sets_SecurityContext_on_service()
        {
            using (Obsolete_MockFactory.Playback())
            {
                SecurityContext securityContext = null;
                Sut = Secured.With(new AlwaysTrueConstraint());
                Sut.Do<BaseService>(service => securityContext = service.SecurityContext);
                Assert.That(securityContext, Is.Not.Null);
            }
        }

        [Test]
        public void Do_proc_secured_with_null_sets_SecurityContext_to_null()
        {
            Obsolete_MockFactory.BackToRecordAll();
            using (Obsolete_MockFactory.Record())
            {
                Expect.Call(() => MockUnitOfWork.Dispose()).Repeat.Never();
            }
            using (Obsolete_MockFactory.Playback())
            {
                var securityContext = new SecurityContext();
                Sut = Secured.With(null);
                Sut.Do<BaseService>(service => securityContext = service.SecurityContext);
                Assert.That(securityContext, Is.Null);
            }
        }
    }
}