namespace Phundus.IdentityAccess.Projections
{
    using Application;
    using Common.Eventing;
    using Common.Projecting;
    using Model.Users;
    using Users.Model;

    public class UserAddressProjection : ProjectionBase<UserAddressData>,
        ISubscribeTo<UserAddressChanged>,
        ISubscribeTo<UserEmailAddressChanged>,
        ISubscribeTo<UserSignedUp>
    {
        public void Handle(UserAddressChanged e)
        {
            Update(e.UserId, x =>
            {
                x.FirstName = e.FirstName;
                x.LastName = e.LastName;
                x.Street = e.Street;
                x.Postcode = e.Postcode;
                x.City = e.City;
                x.PhoneNumber = e.PhoneNumber;
            });
        }

        public void Handle(UserEmailAddressChanged e)
        {
            Update(e.UserId, x =>
                x.EmailAddress = e.NewEmailAddress);
        }

        public void Handle(UserSignedUp e)
        {
            Insert(x =>
            {
                x.UserId = e.UserId;
                x.UserShortId = e.UserShortId;
                x.FirstName = e.FirstName;
                x.LastName = e.LastName;
                x.Street = e.Street;
                x.Postcode = e.Postcode;
                x.City = e.City;
                x.EmailAddress = e.EmailAddress;
                x.PhoneNumber = e.PhoneNumber;
            });
        }
    }
}