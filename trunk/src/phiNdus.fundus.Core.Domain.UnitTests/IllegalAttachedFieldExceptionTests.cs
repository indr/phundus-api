﻿using System;
using NUnit.Framework;
using phiNdus.fundus.TestHelpers;

namespace phiNdus.fundus.Core.Domain.UnitTests
{
    [TestFixture]
    public class IllegalAttachedFieldExceptionTests : ExceptionTestBase<IllegalAttachedFieldException>
    {
        protected override IllegalAttachedFieldException CreateSut()
        {
            return new IllegalAttachedFieldException();
        }

        protected override IllegalAttachedFieldException CreateSut(string message)
        {
            return new IllegalAttachedFieldException(message);
        }

        protected override IllegalAttachedFieldException CreateSut(string message, Exception innerException)
        {
            return new IllegalAttachedFieldException(message, innerException);
        }
    }
}