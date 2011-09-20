using System;
using NUnit.Framework;
using phiNdus.fundus.TestHelpers;

namespace phiNdus.fundus.Core.Business.UnitTests
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