using System;
using System.Reflection;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Business.UnitTests.Services
{
    [TestFixture]
    internal class BaseServiceTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new BaseService();
        }

        #endregion

        protected BaseService Sut { get; set; }

        private static Session CreateSession(User user, string key)
        {
            var info = typeof (Session).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic
                , null, new[] {typeof (User), typeof (string)}, null);
            return (Session) info.Invoke(new object[] {user, key});
        }

        [Test]
        public void Can_set_and_get_Session()
        {
            var session = CreateSession(null, null);
            Assert.That(Sut.Session, Is.Null);
            Sut.Session = session;
            Assert.That(Sut.Session, Is.Not.Null);
            Assert.That(Sut.Session, Is.EqualTo(session));
        }

        [Test]
        public void Set_Session_can_only_be_called_once()
        {
            Sut.Session = null;
            Assert.Throws<InvalidOperationException>(() => Sut.Session = null);
        }
    }
}