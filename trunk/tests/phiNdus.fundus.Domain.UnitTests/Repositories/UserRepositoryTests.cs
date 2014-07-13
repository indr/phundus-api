using NUnit.Framework;
using phiNdus.fundus.Domain.Repositories;

namespace phiNdus.fundus.Domain.UnitTests.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        [Test]
        public void Can_create()
        {
            IUserRepository sut = new UserRepository();
        }
    }
}