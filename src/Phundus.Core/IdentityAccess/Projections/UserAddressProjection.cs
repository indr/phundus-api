namespace Phundus.IdentityAccess.Projections
{
    using System;
    using Common.Domain.Model;
    using Common.Notifications;
    using Common.Projecting;
    using Cqrs;
    using Model.Users;
    using Users.Model;

    public class UserAddressProjection : ProjectionBase<UserAddressData>, IStoredEventsConsumer
    {
        public void Handle(DomainEvent e)
        {
            Process((dynamic) e);
        }

        private void Process(DomainEvent e)
        {
            // Noop
        }

        private void Process(UserSignedUp e)
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

        private void Process(UserAddressChanged e)
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

        private void Process(UserEmailAddressChanged e)
        {
            Update(e.UserId, x =>
                x.EmailAddress = e.NewEmailAddress);
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