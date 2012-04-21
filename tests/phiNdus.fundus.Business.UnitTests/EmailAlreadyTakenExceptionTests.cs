﻿using System;
using NUnit.Framework;
using phiNdus.fundus.TestHelpers;
using phiNdus.fundus.TestHelpers.TestBases;

namespace phiNdus.fundus.Business.UnitTests
{
    [TestFixture]
    public class EmailAlreadyTakenExceptionTests : ExceptionTestBase<EmailAlreadyTakenException>
    {
        protected override EmailAlreadyTakenException CreateSut()
        {
            return new EmailAlreadyTakenException();
        }

        protected override EmailAlreadyTakenException CreateSut(string message)
        {
            return new EmailAlreadyTakenException(message);
        }

        protected override EmailAlreadyTakenException CreateSut(string message, Exception innerException)
        {
            return new EmailAlreadyTakenException(message, innerException);
        }
    }
}