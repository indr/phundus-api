﻿namespace Phundus.IdentityAccess.Organizations.Repositories
{
    using System;
    using Infrastructure;
    using Model;

    public interface IMembershipRequestRepository : IRepository<MembershipApplication>
    {
        MembershipApplication GetById(Guid id);
    }
}