using System.Reflection;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Business.UnitTests.Security
{
    [TestFixture]
    internal class SecuredHelperTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            Session = CreateSession(null, null);
            Sut = new SecuredHelper(Session);
        }

        #endregion

        private static Session CreateSession(User user, string key)
        {
            var info = typeof (Session).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic
                , null, new[] {typeof (User), typeof (string)}, null);
            return (Session) info.Invoke(new object[] {user, key});
        }

        private SecuredHelper Sut { get; set; }
        private Session Session { get; set; }

        [Test]
        public void Call_func_instantiates_service()
        {
            BaseService serviceRef = null;
            Sut.Call<BaseService, int>(service =>
                                           {
                                               serviceRef = service;
                                               return 0;
                                           });
            Assert.That(serviceRef, Is.Not.Null);
        }

        [Test]
        public void Call_func_invokes_lambda()
        {
            var invoked = false;
            Sut.Call<BaseService, int>(service =>
                                           {
                                               invoked = true;
                                               return 0;
                                           });
            Assert.That(invoked, Is.True);
        }

        [Test]
        public void Call_func_sets_Session_on_service()
        {
            Session session = null;
            Sut.Call<BaseService, int>(service =>
                                           {
                                               session = service.Session;
                                               return 0;
                                           });
            Assert.That(session, Is.Not.Null);
            Assert.That(session, Is.EqualTo(Session));
        }

        [Test]
        public void Call_proc_instantiates_service()
        {
            BaseService serviceRef = null;
            Sut.Call<BaseService>(service => serviceRef = service);
            Assert.That(serviceRef, Is.Not.Null);
        }

        [Test]
        public void Call_proc_invokes_lambda()
        {
            var invoked = false;
            Sut.Call<BaseService>(service => { invoked = true; });
            Assert.That(invoked, Is.True);
        }

        [Test]
        public void Call_proc_sets_Session_on_service()
        {
            Session session = null;
            Sut.Call<BaseService>(service => session = service.Session);
            Assert.That(session, Is.Not.Null);
            Assert.That(session, Is.EqualTo(Session));
        }
    }
}