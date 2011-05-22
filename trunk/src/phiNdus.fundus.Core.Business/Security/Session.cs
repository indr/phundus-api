using System;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business.Security
{
    public class Session
    {
        private Session(User user, string key)
        {
            User = user;
            Key = key;
        }

        public User User { get; private set; }
        public string Key { get; private set; }

        public static Session FromKey(string key)
        {
            Guard.Against<ArgumentNullException>(key == null, "key");

            User user;
            using (UnitOfWork.Start())
            {
                var repo = IoC.Resolve<IUserRepository>();
                user = repo.FindBySessionKey(key);
            }
            Guard.Against<InvalidSessionKeyException>(user == null, "");

            return new Session(user, key);
        }
    }
}