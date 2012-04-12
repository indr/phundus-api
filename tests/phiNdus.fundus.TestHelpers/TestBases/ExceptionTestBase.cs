using System;
using NUnit.Framework;

namespace phiNdus.fundus.TestHelpers.TestBases
{
    public abstract class ExceptionTestBase<T> where T : Exception, new()
    {
        protected abstract T CreateSut();
        protected abstract T CreateSut(string message);
        protected abstract T CreateSut(string message, Exception innerException);

        [Test]
        public virtual void Can_create()
        {
            var sut = CreateSut();
            Assert.That(sut, Is.Not.Null);
        }
        
        [Test]
        public virtual void Can_create_with_message()
        {
            var sut = CreateSut("message");
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Message, Is.EqualTo("message"));
        }

        [Test]
        public virtual void Can_create_with_message_and_inner_exception()
        {
            var innerException = new Exception();
            var sut = CreateSut("message", innerException);
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Message, Is.EqualTo("message"));
            Assert.That(sut.InnerException, Is.SameAs(innerException));
        }

        [Test]
        public virtual void Is_derived_from_Exception()
        {
            var sut = CreateSut();
            Assert.That(sut, Is.AssignableTo(typeof (Exception)));
        }
    }
}