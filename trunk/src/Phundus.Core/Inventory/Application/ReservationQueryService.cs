namespace Phundus.Core.Inventory.Application
{
    using System.Collections.Generic;
    using Common.Port.Adapter.Persistence;
    using Data;
    using Domain.Model.Catalog;
    using Domain.Model.Reservations;
    using IdentityAndAccess.Domain.Model.Organizations;

    public interface IReservationQueryService
    {
        ReservationData ReservationDataOfId(OrganizationId organizationId, ArticleId articleId,
            ReservationId reservationId);

        IEnumerable<ReservationData> AllReservationDataByArticle(OrganizationId organizationId, ArticleId articleId);
    }

    public class ReservationQueryService : NHibernateQueryServiceBase<ReservationData>, IReservationQueryService
    {
        public ReservationData ReservationDataOfId(OrganizationId organizationId, ArticleId articleId,
            ReservationId reservationId)
        {
            var query = from r in Query
                where
                    r.OrganizationId == organizationId.Id && r.ArticleId == articleId.Id &&
                    r.ReservationId == reservationId.Id
                select r;
            return query.SingleOrDefault();
        }

        public IEnumerable<ReservationData> AllReservationDataByArticle(OrganizationId organizationId,
            ArticleId articleId)
        {
            var query = from r in Query
                where r.OrganizationId == organizationId.Id && r.ArticleId == articleId.Id
                select r;
            return query.List();
        }
    }
}