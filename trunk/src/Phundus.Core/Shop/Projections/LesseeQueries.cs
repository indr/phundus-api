namespace Phundus.Shop.Projections
{
    using System;
    using Common;
    using Common.Querying;
    using Integration.IdentityAccess;

    public interface ILesseeQueries
    {
        LesseeData GetById(Guid lesseeId);
    }

    public class LesseeQueries : QueryBase, ILesseeQueries
    {
        private readonly IUsersQueries _usersQueries;

        public LesseeQueries(IUsersQueries usersQueries)
        {            
            _usersQueries = usersQueries;
        }

        public LesseeData GetById(Guid lesseeId)
        {
            var user = _usersQueries.FindById(lesseeId);
            if (user == null)
                throw new NotFoundException("Lessee {0} not found", lesseeId);

            return new LesseeData(user.UserId, user.FullName, user.Street + "\n" + user.Postcode + " " + user.City,
                user.MobilePhone, user.EmailAddress);
        }
    }
}