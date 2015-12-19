namespace Phundus.Specs.Rest
{
    using System;
    using System.Net;
    using RestSharp;

    public class PhundusApiAuthenticator : IAuthenticator
    {
        public class SessionDoc
        {
            public string password { get; set; }
            public string username { get; set; }
        }

        private readonly string _password;
        private readonly string _userName;

        public PhundusApiAuthenticator(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            if (request.Resource == "sessions")
                return;

            var r = new RestRequest("sessions", Method.POST);
            r.RequestFormat = DataFormat.Json;
            r.AddBody(new SessionDoc {username = _userName, password = _password});
            r.UserState = "MyAuth";
            var re = client.Execute(r);

            if (re.StatusCode != HttpStatusCode.OK)
                throw new Exception("Could not authenticate: " + re.StatusCode.ToString() + " " + re.StatusDescription);

            foreach (var each in re.Cookies)
                request.AddCookie(each.Name, each.Value);
        }
    }
}