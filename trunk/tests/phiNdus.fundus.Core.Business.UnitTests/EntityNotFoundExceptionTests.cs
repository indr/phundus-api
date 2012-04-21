using System;
using NUnit.Framework;
using phiNdus.fundus.TestHelpers;
using phiNdus.fundus.TestHelpers.TestBases;

namespace phiNdus.fundus.Core.Business.UnitTests
{
    [TestFixture]
    public class EntityNotFoundExceptionTests : ExceptionTestBase<EntityNotFoundException>
    {
        protected override EntityNotFoundException CreateSut()
        {
            return new EntityNotFoundException();
        }

        protected override EntityNotFoundException CreateSut(string message)
        {
            return new EntityNotFoundException(message);
        }

        protected override EntityNotFoundException CreateSut(string message, Exception innerException)
        {
            return new EntityNotFoundException(message, innerException);
        }
    }
}