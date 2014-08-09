namespace Phundus.Specs.Rest
{
    using RestSharp;

    public class PhundusApiAuthenticator : IAuthenticator
    {
        public class SessionDoc
        {
            public string Password { get; set; }
            public string Username { get; set; }
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
            r.AddBody(new SessionDoc {Username = _userName, Password = _password});
            r.UserState = "MyAuth";
            var re = client.Execute(r);


            foreach (var each in re.Cookies)
                request.AddCookie(each.Name, each.Value);
        }
    }
}