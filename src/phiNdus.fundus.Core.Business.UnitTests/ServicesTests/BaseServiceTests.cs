using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Security;
using phiNdus.fundus.Core.Business.Services;
using phiNdus.fundus.Core.Business.UnitTests.Security;
using phiNdus.fundus.TestHelpers.Builders;

namespace phiNdus.fundus.Core.Business.UnitTests.Services
{
    [TestFixture]
    public class BaseServiceTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new BaseService();
        }

        #endregion

        protected BaseService Sut { get; set; }

        [Test]
        public void Can_set_and_get_SecurityContextr()
        {
            var context = new SecurityContext();
            context.SecuritySession = new SecuritySessionBuilder(null, null).Build();
            Assert.That(Sut.SecurityContext, Is.Null);
            Sut.SecurityContext = context;
            Assert.That(Sut.SecurityContext, Is.Not.Null);
            Assert.That(Sut.SecurityContext, Is.EqualTo(context));
        }

        [Test]
        public void Set_SecurityContext_can_only_be_called_once()
        {
            Sut.SecurityContext = null;
            Assert.Throws<InvalidOperationException>(() => Sut.SecurityContext = null);
        }
    }
}