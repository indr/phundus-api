﻿using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Repositories;

namespace phiNdus.fundus.Core.Domain.UnitTests.Repositories
{
    [TestFixture]
    internal class UserRepositoryTests
    {
        [Test]
        public void Can_create()
        {
            IUserRepository sut = new UserRepository();
        }
    }
}