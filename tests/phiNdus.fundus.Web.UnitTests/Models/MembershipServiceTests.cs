using System;
using System.Web.Security;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Web.Models;
using phiNdus.fundus.TestHelpers;
using Rhino.Mocks;

namespace phiNdus.fundus.Web.UnitTests.Models
{
    [TestFixture]
    public class MembershipServiceTests : MockTestBase<IMembershipService>
    {
        private MembershipProvider MockMembershipProvider { get; set; }

        protected override void RegisterDependencies(IWindsorContainer container)
        {
            MockMembershipProvider = MockFactory.StrictMock<MembershipProvider>();
        }

        protected override IMembershipService CreateSut()
        {
            return new MembershipService(MockMembershipProvider);
        }

        [Test]
        public void CreateUser_should_relay_to_MembershipProvider()
        {
            Assert.Ignore("Wir müssen uns mal unterhalten...");

            using (MockFactory.Record())
            {
                MembershipCreateStatus status;
                Expect.Call(MockMembershipProvider.CreateUser("dave@example.com", "password", "dave@example.com", null,
                                                              null, false, null, out status)).Return(null);
            }

            using (MockFactory.Playback())
            {
                MembershipCreateStatus status;
                MembershipUser actual = Sut.CreateUser("dave@example.com", "password", "", "", out status);
                Assert.That(actual, Is.Null);
            }
        }

        [Test]
        public void CreateUser_with_empty_email_throws()
        {
            MembershipCreateStatus status;
            var ex = Assert.Throws<ArgumentException>(() => Sut.CreateUser("", "password", "", "", out status));
            Assert.That(ex.ParamName, Is.EqualTo("email"));
        }

        [Test]
        public void CreateUser_with_empty_password_throws()
        {
            MembershipCreateStatus status;
            var ex = Assert.Throws<ArgumentException>(() => Sut.CreateUser("dave@example.com", "", "", "", out status));
            Assert.That(ex.ParamName, Is.EqualTo("password"));
        }

        [Test]
        public void CreateUser_with_white_space_email_throws()
        {
            MembershipCreateStatus status;
            var ex = Assert.Throws<ArgumentException>(() => Sut.CreateUser(" ", "password", "", "", out status));
            Assert.That(ex.ParamName, Is.EqualTo("email"));
        }

        [Test]
        public void CreateUser_with_white_space_password_throws()
        {
            MembershipCreateStatus status;
            var ex = Assert.Throws<ArgumentException>(() => Sut.CreateUser("dave@example.com", " ", "", "", out status));
            Assert.That(ex.ParamName, Is.EqualTo("password"));
        }

        [Test]
        public void ValidateUser_should_relay_to_MembershipProvider()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockMembershipProvider.ValidateUser("dave@example.com", "password")).Return(true);
            }

            using (MockFactory.Playback())
            {
                bool actual = Sut.ValidateUser("dave@example.com", "password");
                Assert.That(actual, Is.True);
            }
        }

        [Test]
        public void ValidateUser_with_empty_email_throws()
        {
            var ex = Assert.Throws<ArgumentException>(() => Sut.ValidateUser("", "password"));
            Assert.That(ex.ParamName, Is.EqualTo("email"));
        }

        [Test]
        public void ValidateUser_with_empty_password_throws()
        {
            var ex = Assert.Throws<ArgumentException>(() => Sut.ValidateUser("dave@example.com", ""));
            Assert.That(ex.ParamName, Is.EqualTo("password"));
        }


        [Test]
        public void ValidateUser_with_white_space_email_throws()
        {
            var ex = Assert.Throws<ArgumentException>(() => Sut.ValidateUser(" ", "password"));
            Assert.That(ex.ParamName, Is.EqualTo("email"));
        }

        [Test]
        public void ValidateUser_with_white_space_password_throws()
        {
            var ex = Assert.Throws<ArgumentException>(() => Sut.ValidateUser("dave@example.com", " "));
            Assert.That(ex.ParamName, Is.EqualTo("password"));
        }
    }
}