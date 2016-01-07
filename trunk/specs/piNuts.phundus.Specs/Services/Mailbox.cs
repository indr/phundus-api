namespace Phundus.Specs.Services
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using Api;
    using OpenPop.Mime;
    using OpenPop.Pop3;
    using Phundus.Rest.ContentObjects;

    public interface IMailbox
    {
        Mail Find(string subject, string toAddress);
    }

    public class RestMailbox : AppBase, IMailbox
    {
        private readonly ApiClient _apiClient;

        public RestMailbox(ApiClient apiClient)
        {
            if (apiClient == null) throw new ArgumentNullException("apiClient");
            _apiClient = apiClient;
        }

        public Mail Find(string subject, string toAddress)
        {
            var response = _apiClient.For<MailsApi>().Query();
            AssertHttpStatus(HttpStatusCode.OK, response);

            return response.Data.Results.FirstOrDefault(p => p.Subject == subject && p.To.Contains(toAddress));
        }
    }

    public class PopMailbox : IMailbox
    {
        private readonly TimeSpan _delayPeriod;
        private readonly TimeSpan _pause;

        public PopMailbox()
        {
            _delayPeriod = TimeSpan.FromSeconds(20);
            _pause = TimeSpan.FromSeconds(1);
        }

        public Mail Find(string subject, string toAddress)
        {
            var start = DateTime.Now;
            do
            {
                using (var client = GetClient(toAddress))
                {
                    try
                    {
                        var count = client.GetMessageCount();
                        for (var i = 1; i <= count; i++)
                        {
                            var msg = client.GetMessageHeaders(i);
                            if ((msg.Subject != null) && (msg.Subject.Equals(subject)))
                                return ToMail(client.GetMessage(i));
                        }
                    }
                    finally
                    {
                        client.DeleteAllMessages();
                    }
                }
                Thread.Sleep(_pause);
            } while (DateTime.Now < start + _delayPeriod);
            return null;
        }

        private static Pop3Client GetClient(string toAddress)
        {
            var result = new Pop3Client();
            result.Connect("phundus.ch", 110, false);
            result.Authenticate(toAddress, "p@ss_w03d");
            return result;
        }

        private static Mail ToMail(Message message)
        {
            var textBody = message.FindFirstPlainTextVersion();
            var htmlBody = message.FindFirstHtmlVersion();
            return new Mail
            {
                MailId = message.Headers.MessageId,
                Date = message.Headers.DateSent,
                From = message.Headers.From.Address,
                Subject = message.Headers.Subject,
                To = message.Headers.To.Select(s => s.Address).ToList(),
                TextBody = textBody == null ? null : textBody.GetBodyAsText(),
                HtmlBody = htmlBody == null ? null : htmlBody.GetBodyAsText()
            };
        }
    }
}