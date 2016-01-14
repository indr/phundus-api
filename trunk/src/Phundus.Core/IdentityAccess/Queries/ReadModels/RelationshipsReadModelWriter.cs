namespace Phundus.IdentityAccess.Queries.ReadModels
{
    using System;
    using System.Linq;
    using Ddd;
    using Organizations.Model;

    public class RelationshipsReadModelWriter : ReadModelWriterBase, ISubscribeTo<MembershipApplicationFiled>,
        ISubscribeTo<MembershipApplicationApproved>, ISubscribeTo<MembershipApplicationRejected>
    {
        public void Handle(MembershipApplicationApproved @event)
        {
            UpdateOrInsert(@event.UserId, @event.OrganizationId, @event.OccuredOnUtc, RelationshipStatusDto.Member);
        }

        public void Handle(MembershipApplicationFiled @event)
        {
            UpdateOrInsert(@event.UserId, @event.OrganizationId, @event.OccuredOnUtc, RelationshipStatusDto.Application);
        }

        public void Handle(MembershipApplicationRejected @event)
        {
            UpdateOrInsert(@event.UserId, @event.OrganizationId, @event.OccuredOnUtc, RelationshipStatusDto.Rejected);
        }

        private void UpdateOrInsert(int userId, Guid organizationId, DateTime timestamp, RelationshipStatusDto status)
        {
            var entity = (from r in Ctx.RelationshipDtos
                where r.OrganizationGuid == organizationId && r.UserId == userId
                select r).SingleOrDefault();

            if (entity == null)
            {
                entity = new RelationshipDto { OrganizationGuid = organizationId, UserId = userId };
                Ctx.RelationshipDtos.InsertOnSubmit(entity);
            }

            entity.Timestamp = timestamp;
            entity.Status = status;

            Ctx.SubmitChanges();
        }
    }
}