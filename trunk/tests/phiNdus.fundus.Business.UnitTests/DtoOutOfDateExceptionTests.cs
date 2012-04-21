using System;
using NUnit.Framework;
using phiNdus.fundus.TestHelpers;
using phiNdus.fundus.TestHelpers.TestBases;

namespace phiNdus.fundus.Business.UnitTests
{
    [TestFixture]
    public class DtoOutOfDateExceptionTests : ExceptionTestBase<DtoOutOfDateException>
    {
        protected override DtoOutOfDateException CreateSut()
        {
            return new DtoOutOfDateException();
        }

        protected override DtoOutOfDateException CreateSut(string message)
        {
            return new DtoOutOfDateException(message);
        }

        protected override DtoOutOfDateException CreateSut(string message, Exception innerException)
        {
            return new DtoOutOfDateException(message, innerException);
        }
    }
}