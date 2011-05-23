using System.Threading;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Gateways;
using phiNdus.fundus.Core.Business.IntegrationTests.Helpers;
using phiNdus.fundus.Core.Business.IntegrationTests.TestHelpers;

namespace phiNdus.fundus.Core.Business.IntegrationTests.Gateways
{
    [TestFixture]
    public class MailGatewayTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new MailGateway(FromHost, FromUserName, FromPassword, FromAddress);
        }

        #endregion

        private const string FromHost = "mail.indr.ch";
        private const string FromUserName = "fundus-sys-test-1@indr.ch";
        private const string FromPassword = "phiNdus";
        private const string FromAddress = "fundus-sys-test-1@indr.ch";

        private const string ToHost = "mail.indr.ch";
        private const string ToUserName = "fundus-sys-test-2@indr.ch";
        private const string ToPassword = "phiNdus";
        private const string ToAddress = "fundus-sys-test-2@indr.ch";

        private IMailGateway Sut { get; set; }


        private static Pop3Message GetFromMailBySubject(string subject)
        {
            return RetrieveMail(FromHost, FromUserName, FromPassword, subject);
        }

        private static Pop3Message GetToMailBySubject(string subject)
        {
            return RetrieveMail(ToHost, ToUserName, ToPassword, subject);
        }

        private static Pop3Message RetrieveMail(string host, string username, string password, string subject)
        {
            var pop = new Pop3();
            pop.Connect(host, username, password);
            var result = pop.Find(subject);
            pop.DeleteAll();
            pop.Disconnect();
            return result;
        }

        [Test]
        public void Can_send()
        {
            Sut.Send(ToAddress, "[MailGatewayTests] Can_send", "");
            Thread.Sleep(2000);
            Assert.That(GetToMailBySubject("[MailGatewayTests] Can_send"), Is.Not.Null, "Could not retrieve mail");
        }
    }
}