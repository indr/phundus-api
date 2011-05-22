using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Settings;

namespace phiNdus.fundus.Core.Domain.UnitTests.Settings
{
    [TestFixture]
    internal class MailTemplatesSettingsTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new MailTemplatesSettings("");
        }

        #endregion

        private IMailTemplatesSettings Sut { get; set; }

        [Test]
        public void Can_get_UserAccountValidation()
        {
            var template = Sut.UserAccountValidation;
            Assert.That(template, Is.Not.Null);
        }
    }
}