namespace Phundus.Core.IdentityAndAccess.Queries
{
    using System;
    using System.Linq;
    using Common.EventPublishing;
    using Ddd;
    using Organizations.Model;

    public class RelationshipsReadModelWriter : ReadModelWriterBase, ISubscribeTo<MembershipApplicationFiled>,
        ISubscribeTo<MembershipApplicationApproved>, ISubscribeTo<MembershipApplicationRejected>
    {
        public void Handle(MembershipApplicationApproved @event)
        {
            UpdateOrInsert(@event.UserId, @event.OrganizationId, @event.OccuredOnUtc, RelationshipStatusDto.Application);
        }

        public void Handle(MembershipApplicationFiled @event)
        {
            UpdateOrInsert(@event.UserId, @event.OrganizationId, @event.OccuredOnUtc, RelationshipStatusDto.Application);
        }

        public void Handle(MembershipApplicationRejected @event)
        {
            UpdateOrInsert(@event.UserId, @event.OrganizationId, @event.OccuredOnUtc, RelationshipStatusDto.Application);
        }

        private void UpdateOrInsert(int userId, int organizationId, DateTime timestamp, RelationshipStatusDto status)
        {
            var entity = (from r in Ctx.RelationshipDtos
                where r.OrganizationId == organizationId && r.UserId == userId
                select r).SingleOrDefault();

            if (entity == null)
            {
                entity = new RelationshipDto {OrganizationId = organizationId, UserId = userId};
                Ctx.RelationshipDtos.InsertOnSubmit(entity);
            }

            entity.Timestamp = timestamp;
            entity.Status = status;

            Ctx.SubmitChanges();
        }
    }
}