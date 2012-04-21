using System;
using NUnit.Framework;
using phiNdus.fundus.TestHelpers;
using phiNdus.fundus.TestHelpers.TestBases;

namespace phiNdus.fundus.Domain.UnitTests
{
    [TestFixture]
    public class FieldAlreadyAttachedExceptionTests : ExceptionTestBase<FieldAlreadyAttachedException>
    {
        protected override FieldAlreadyAttachedException CreateSut()
        {
            return new FieldAlreadyAttachedException();
        }

        protected override FieldAlreadyAttachedException CreateSut(string message)
        {
            return new FieldAlreadyAttachedException(message);
        }

        protected override FieldAlreadyAttachedException CreateSut(string message, Exception innerException)
        {
            return new FieldAlreadyAttachedException(message, innerException);
        }
    }
}