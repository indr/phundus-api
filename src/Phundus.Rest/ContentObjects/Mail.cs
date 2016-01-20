namespace Phundus.Rest.ContentObjects
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Mail
    {
        [JsonProperty("mailId")]
        public string MailId { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public List<string> To { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("textBody")]
        public string TextBody { get; set; }

        [JsonProperty("hasTextPart")]
        public bool HasTextPart { get { return !String.IsNullOrWhiteSpace(TextBody); } }

        [JsonProperty("htmlBody")]
        public string HtmlBody { get; set; }

        [JsonProperty("hasHtmlPart")]
        public bool HasHtmlPart { get { return !String.IsNullOrWhiteSpace(HtmlBody); } }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}