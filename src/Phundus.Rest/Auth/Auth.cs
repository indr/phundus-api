namespace Phundus.Rest.Auth
{
    using System;
    using System.Collections.Generic;

    public class AuthConfig
    {
        private readonly IDictionary<string, IList<string>> _accessLevels = new Dictionary<string, IList<string>>();
        private readonly IList<string> _roles = new[] {"public", "user", "manager", "admin"};

        public AuthConfig()
        {
            _accessLevels.Add("public", new[] {"*"});
            _accessLevels.Add("anon", new[] {"public"});
            _accessLevels.Add("user", new[] {"user", "manager"});
            _accessLevels.Add("manager", new[] {"manager", "admin"});
            _accessLevels.Add("admin", new[] {"admin"});
        }

        public IDictionary<string, IList<string>> AccessLevels
        {
            get { return _accessLevels; }
        }

        public IEnumerable<string> Roles
        {
            get { return _roles; }
        }
    }

    public class Auth
    {
        private readonly IDictionary<string, UserRole> _userRoles;

        public Auth()
        {
            var config = new AuthConfig();

            _userRoles = BuildUserRoles(config.Roles);
        }

        public IDictionary<string, UserRole> Roles
        {
            get { return _userRoles; }
        }

        private static IDictionary<string, UserRole> BuildUserRoles(IEnumerable<string> roles)
        {
            var id = 0;
            var bitMask = "01";
            var result = new Dictionary<string, UserRole>();

            foreach (var each in roles)
            {
                var intCode = Convert.ToInt32(bitMask, 2);
                result.Add(each, new UserRole
                {
                    Id = id++,
                    BitMask = intCode,
                    Title = each
                });
                bitMask = Convert.ToString(intCode << 1, 2);
            }

            return result;
        }
    }

    public class UserRole
    {
        public int BitMask;
        public int Id;
        public string Title;
    }
}