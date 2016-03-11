namespace Phundus.Common.Mailing
{
    public abstract class MailTemplates
    {
        public const string TextSignature = @"

--
This is an automatically generated message from phundus.
-
If you think it was sent incorrectly contact the administrators at lukas.mueller@phundus.ch or reto.inderbitzin@phundus.ch.";

        public const string HtmlFooter = @"<hr />
<footer>
    <p>This is an automatically generated message from phundus.<br />If you think it was sent incorrectly contact the administrators at lukas.mueller@phundus.ch or reto.inderbitzin@phundus.ch.</p>
</footer>
</div>
</body>
</html>
";

        public const string HtmlHeader = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN""
    ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"" lang=""en"">
<head>
    <meta http-equiv=""Content-Type"" content=""text/html;charset=utf-8"" />
    <link rel=""stylesheet"" type=""text/css"" href=""http://@Model.Urls.ServerUrl/Content/bootstrap.min.css"" />
    <link rel=""stylesheet"" type=""text/css"" href=""http://@Model.Urls.ServerUrl/Content/fundus.css"" />
    <style type=""text/css"">
    body { margin: 0; padding: 0; color: #333333; font-family: Helvetica Neue,Helvetica,Arial,sans-serif; font-size: 13px; }
    </style>
</head>
<body>
<div class=""container"" style=""margin: 10px; padding: 0;"">";
    }
}