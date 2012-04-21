﻿using System;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Business.Security.Constraints
{
    public static class User
    {
        public static AbstractConstraint InRole(Role role)
        {
            return new UserInRoleConstraint(role);
        }

        public static AbstractConstraint HasEmail(string email)
        {
            return new UserHasEmailConstraint(email);
        }

        public static AbstractConstraint Is(int id)
        {
            return new UserIsContraint(id);
        }
    }
}