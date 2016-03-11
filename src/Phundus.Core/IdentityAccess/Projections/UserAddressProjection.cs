namespace Phundus.IdentityAccess.Projections
{
    using System;
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

    public class UserAddressData
    {
        public virtual Guid UserId { get; set; }
        public virtual int UserShortId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Street { get; set; }
        public virtual string Postcode { get; set; }
        public virtual string City { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string PhoneNumber { get; set; }
    }
}