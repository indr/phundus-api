namespace Phundus.Rest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Authentication;
    using System.Security.Principal;
    using System.Text.RegularExpressions;
    using System.Web.Http;
    using System.Web.Security;
    using Auth;
    using AutoMapper;
    using Castle.Core.Logging;
    using Common;
    using Common.Commanding;
    using Common.Domain.Model;
    using Common.Messaging;

    public class ApiControllerBase : ApiController
    {
        private ILogger _log = NullLogger.Instance;

        public ILogger Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public IBus Bus { get; set; }

        public IIdentity Identity { get; set; }

        public ICommandDispatcher Dispatcher { get; set; }

        protected CurrentUserId CurrentUserId
        {
            get
            {
                var user = Membership.GetUser();
                if (user == null)
                    throw new AuthenticationException();
                return new ProviderUserKey(user.ProviderUserKey).UserId;
            }
        }

        protected CurrentUserId CurrentUserIdOrNull
        {
            get
            {
                var user = Membership.GetUser();
                if (user == null)
                    return null;
                return new ProviderUserKey(user.ProviderUserKey).UserId;
            }
        }

        protected void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            Dispatcher.Dispatch(command);
        }

        protected static TDestination Map<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }

        protected HttpResponseMessage Accepted(AsyncCommand command)
        {
            return Request.CreateResponse(HttpStatusCode.Accepted, new
            {
                commandId = command.CommandId,
                createdAtUtc = command.CreatedAtUtc
            });
        }

        protected HttpResponseMessage Created<T>(T content)
        {
            return Request.CreateResponse(HttpStatusCode.Created, content);
        }

        protected HttpResponseMessage Ok<T>(T content)
        {
            return Request.CreateResponse(HttpStatusCode.OK, content);
        }

        protected HttpResponseMessage NoContent()
        {
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        protected HttpResponseMessage CreateNotFoundResponse(string format, object arg0)
        {
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(format, arg0));
        }

        protected HttpResponseMessage CreatePdfResponse(Stream stream, string fileName)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = string.Format(fileName)
            };

            return result;
        }

        protected Dictionary<string, string> GetQueryParams()
        {
            return Request.GetQueryNameValuePairs().ToDictionary(ks => ks.Key, es => es.Value);
        }

        protected void CheckForMaintenanceMode(string emailAddress)
        {
            if ((Config.InMaintenance) &&
                (!Regex.Match(emailAddress, @"@(test\.)?phundus\.ch$", RegexOptions.IgnoreCase).Success))
                throw new MaintenanceModeException("Das System befindet sich im Wartungsmodus.");
        }
    }
}