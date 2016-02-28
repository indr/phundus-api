namespace Phundus.IdentityAccess.Projections
{
    using System;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Model.Users;
    using Users.Model;

    public class UserAddressProjection : ProjectionBase<UserAddress>, IStoredEventsConsumer
    {
        public void Handle(DomainEvent e)
        {
            Process((dynamic)e);
        }

        private void Process(DomainEvent e)
        {
            // Noop
        }

        private void Process(UserSignedUp e)
        {
            Insert(x =>
            {
                x.UserId = e.UserGuid;
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
            Update(e.UserGuid, x =>
                x.EmailAddress = e.NewEmailAddress);
        }
    }

    public class UserAddress
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}