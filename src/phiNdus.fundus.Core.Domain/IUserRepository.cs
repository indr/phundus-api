﻿using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain
{
    public interface IUserRepository : IRepository<User>
    {
        User FindByEmail(string email);
    }
}