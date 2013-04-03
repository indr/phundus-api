﻿using System;
using NHibernate;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Security
{
    using phiNdus.fundus.Domain;
    using Rhino.Commons;
    using piNuts.phundus.Infrastructure;
    using piNuts.phundus.Infrastructure.Rhino;

    public class SecuritySession
    {
        private SecuritySession(User user, string key)
        {
            User = user;
            Key = key;
        }

        public User User { get; private set; }
        public string Key { get; private set; }

        public static SecuritySession FromKey(string key)
        {
            Guard.Against<ArgumentNullException>(key == null, "key");

            User user;
            using (UnitOfWork.Start())
            {
                var repo = GlobalContainer.Resolve<IUserRepository>();
                user = repo.FindBySessionKey(key);

                Guard.Against<InvalidSessionKeyException>(user == null, "");

                NHibernateUtil.Initialize(user.Memberships);
            }
            

            return new SecuritySession(user, key);
        }
    }
}