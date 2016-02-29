namespace Phundus.IdentityAccess.Projections
{
    using Common;
    using Common.Domain.Model;
    using Common.Notifications;
    using Cqrs;
    using Model;
    using Model.Organizations;
    using Organizations.Model;

    public class OrganizationProjection : ProjectionBase<OrganizationData>, IStoredEventsConsumer
    {
        public void Handle(DomainEvent e)
        {
            Process((dynamic)e);
        }

        private void Process(DomainEvent e)
        {
            // Noop
        }

        private void Process(OrganizationEstablished e)
        {
            Insert(x =>
            {
                x.OrganizationId = e.OrganizationId;
                x.Name = e.Name;
                x.Url = e.Name.ToFriendlyUrl();
                x.Plan = e.Plan;
                x.EstablishedAtUtc = e.OccuredOnUtc;
            });
        }

        private void Process(OrganizationPlanChanged e)
        {
            Update(e.OrganizationId, x =>
                x.Plan = e.NewPlan);
        }

        private void Process(OrganizationContactDetailsChanged e)
        {
            Update(e.OrganizationId, x =>
            {
                x.Line1 = e.Line1;
                x.Line2 = e.Line2;
                x.Street = e.Street;
                x.Postcode = e.Postcode;
                x.City = e.City;
                x.PhoneNumber = e.PhoneNumber;
                x.EmailAddress = e.EmailAddress;
                x.Website = e.Website;
            });
        }

        private void Process(PublicRentalSettingChanged e)
        {
            Update(e.OrganizationId, x =>
                x.PublicRental = e.Value);
        }
    }
}