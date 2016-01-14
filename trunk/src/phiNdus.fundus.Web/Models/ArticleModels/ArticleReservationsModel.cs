namespace Phundus.Web.Models.ArticleModels
{
    using System.Collections.Generic;
    using Inventory.AvailabilityAndReservation.Model;

    public class ArticleReservationsModel
    {
        public IEnumerable<Reservation> Items { get; set; }
    }
}