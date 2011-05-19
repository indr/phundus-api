using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Repositories;

namespace phiNdus.fundus.Core.Domain.UnitTests.Repositories
{
    [TestFixture]
    internal class SettingRepositoryTests
    {
        [Test]
        public void Can_create()
        {
            ISettingRepository sut = new SettingRepository();
        }
    }
}