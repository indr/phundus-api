﻿namespace Phundus.Shop.Model
{
    using System;
    using Common;
    using Common.Domain.Model;
    using Integration.IdentityAccess;

    public interface ILesseeService
    {        
        Lessee GetById(LesseeId lesseeId);
    }

    public class LesseeService : ILesseeService
    {
        private readonly IUsersQueries _usersQueries;

        public LesseeService(IUsersQueries usersQueries)
        {            
            _usersQueries = usersQueries;
        }

        public Lessee GetById(LesseeId lesseeId)
        {
            if (lesseeId == null) throw new ArgumentNullException("lesseeId");

            var user = _usersQueries.GetById(lesseeId.Id);
            if (user == null)
                throw new NotFoundException(string.Format("Lessee {0} not found.", lesseeId));

            return ToLessee(user);
        }

        private static Lessee ToLessee(IUser user)
        {
            return new Lessee(new LesseeId(user.UserId), user.FirstName, user.LastName, user.Street, user.Postcode,
                user.City, user.EmailAddress, user.MobilePhone, user.JsNummer.ToString());
        }
    }
}