using NUnit.Framework;
using phiNdus.fundus.Domain.Repositories;

namespace phiNdus.fundus.Domain.UnitTests.Repositories
{
    [TestFixture]
    public class RoleRepositoryTests
    {
        [Test]
        public void Can_create()
        {
            new RoleRepository();
        }
    }
}