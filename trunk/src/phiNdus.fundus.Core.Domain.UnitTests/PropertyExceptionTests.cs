using System;
using NUnit.Framework;

namespace phiNdus.fundus.Core.Domain.UnitTests
{
    [TestFixture]
    public class PropertyExceptionTests
    {
        [Test]
        public void Can_create()
        {
            var sut = new PropertyException();
            Assert.That(sut, Is.Not.Null);
        }

        [Test]
        public void Is_derived_from_Exception()
        {
            var sut = new PropertyException();
            Assert.That(sut, Is.InstanceOf(typeof (Exception)));
        }
    }
}