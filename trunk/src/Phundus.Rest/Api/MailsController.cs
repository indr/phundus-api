namespace Phundus.Rest.Api
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;
    using Common;
    using Common.Resources;
    using ContentObjects;
    using MsgReader.Mime;    

    [RoutePrefix("api/mails")]
    public class MailsController : ApiControllerBase
    {
        [GET("")]
        [AllowAnonymous]
        public virtual QueryOkResponseContent<MailCto> Get()
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
            return new QueryOkResponseContent<MailCto>(messages.OrderByDescending(p => p.Date));
        }

        [GET("{mailId}")]
        public virtual HttpResponseMessage Get(string mailId)
        {
            var result = GetMails().SingleOrDefault(p => p.MailId == mailId);
            if (result == null)
                throw new NotFoundException("Mail not found.");

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [GET("{mailId}/html")]
        public virtual HttpResponseMessage GetHtml(string mailId)
        {
            var result = GetMails().SingleOrDefault(p => p.MailId == mailId && p.HasHtmlPart);
            if (result == null)
                throw new NotFoundException("Mail or message part html not found.");

            var response = new HttpResponseMessage();
            response.Content = new StringContent(result.HtmlBody, Encoding.UTF8, "text/html");
            response.Content.Headers.ContentType.CharSet = "utf-8";
            return response;
        }

        [GET("{mailId}/text")]
        public virtual HttpResponseMessage GetText(string mailId)
        {
            var result = GetMails().SingleOrDefault(p => p.MailId == mailId && p.HasTextPart);
            if (result == null)
                throw new NotFoundException("Mail or message part text not found.");

            var response = new HttpResponseMessage();
            response.Content = new StringContent(result.TextBody, Encoding.UTF8, "text/plain");
            response.Content.Headers.ContentType.CharSet = "utf-8";
            return response;
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

        private IEnumerable<MailCto> GetMails()
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

        private static MailCto ToMail(string id, Message message)
        {
            return new MailCto
            {
                MailId = id,
                Date = message.Headers.DateSent,
                Subject = message.Headers.Subject,
                From = message.Headers.From.Address,
                To = message.Headers.To.Select(rma => rma.Address).ToList(),
                TextBody = message.TextBody != null ? message.TextBody.GetBodyAsText() : null,
                HtmlBody = message.HtmlBody != null ? message.HtmlBody.GetBodyAsText() : null
            };
        }
    }
}