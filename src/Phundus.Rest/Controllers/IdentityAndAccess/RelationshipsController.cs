namespace Phundus.Rest.Controllers.IdentityAndAccess
{
    using System;
    using AutoMapper;
    using Castle.Transactions;
    using Core.IdentityAndAccess.Queries;

    public class RelationshipsController : ApiControllerBase
    {
        static RelationshipsController()
        {
            Mapper.CreateMap<RelationshipDto, RelationshipDoc>()
                .ForMember(d => d.OrganizationId, o => o.MapFrom(s => s.OrganizationId))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.UserId))
                .ForMember(d => d.Timestamp, o => o.MapFrom(s => s.Timestamp))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        }

        public IRelationshipQueries RelationshipQueries { get; set; }

        [Transaction]
        public virtual RelationshipDoc Get(int organization)
        {
            return Map<RelationshipDoc>(RelationshipQueries.ByMemberIdForOrganizationId(CurrentUserId, organization));
        }
    }

    public class RelationshipDoc
    {
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }
}