namespace Phundus.Rest.Api
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Common;
    using MsgReader.Mime;
    using Newtonsoft.Json;

    [RoutePrefix("api/mails")]
    [Authorize(Roles = "Admin")]
    public class MailsController : ApiControllerBase
    {
        [GET("")]
        public virtual MailsQueryOkResponseContent Get()
        {
            var messages = GetMails();

            foreach (var each in Request.GetQueryNameValuePairs())
            {
                if (each.Key == "to")
                {
                    var to = each.Value;
                    messages = messages.Where(p => p.To.Contains(to));
                }
            }

            return new MailsQueryOkResponseContent(messages.OrderByDescending(p => p.Date));
        }

        [GET("{id}")]
        public virtual HttpResponseMessage Get(string id)
        {
            var result = GetMails().SingleOrDefault(p => p.Id == id);
            if (result == null)
                throw new NotFoundException("Mail not found.");

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [DELETE("")]
        public virtual HttpResponseMessage Delete()
        {
            foreach (var each in GetFiles())
            {
                File.Delete(each);
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [DELETE("{id}")]
        public virtual HttpResponseMessage Delete(string id)
        {
            File.Delete(Path.Combine(GetPath(), id + ".eml"));
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        private IEnumerable<Mail> GetMails()
        {
            foreach (var each in GetFiles())
            {
                var id = Path.GetFileNameWithoutExtension(each);
                var fileInfo = new FileInfo(each);
                yield return ToMail(id, Message.Load(fileInfo));
            }
        }

        private static IEnumerable<string> GetFiles()
        {
            return Directory.GetFiles(GetPath(), "*.eml");
        }

        private static string GetPath()
        {
            return HttpContext.Current.Server.MapPath(@"~\App_Data\Mails");
        }

        private static Mail ToMail(string id, Message message)
        {
            return new Mail
            {
                Id = id,
                Date = message.Headers.DateSent,
                Subject = message.Headers.Subject,
                From = message.Headers.From.Address,
                To = message.Headers.To.Select(rma => rma.Address).ToList(),
                TextBody = message.TextBody != null ? message.TextBody.GetBodyAsText() : null,
                HtmlBody = message.HtmlBody != null ? message.HtmlBody.GetBodyAsText() : null
            };
        }
    }

    public class MailsQueryOkResponseContent : List<Mail>
    {
        public MailsQueryOkResponseContent(IEnumerable<Mail> collection)
            : base(collection)
        {
        }
    }

    public class Mail
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public IList<string> To { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("textBody")]
        public string TextBody { get; set; }

        [JsonProperty("htmlBody")]
        public string HtmlBody { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}