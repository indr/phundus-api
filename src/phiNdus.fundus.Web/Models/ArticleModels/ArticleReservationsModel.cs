using System.Collections.Generic;

namespace phiNdus.fundus.Web.Models.ArticleModels
{
    using Phundus.Core.Inventory.AvailabilityAndReservation.Model;
    using Phundus.Core.ReservationCtx;

    public class ArticleReservationsModel
    {
        public IEnumerable<Reservation> Items { get; set; }
    }
}