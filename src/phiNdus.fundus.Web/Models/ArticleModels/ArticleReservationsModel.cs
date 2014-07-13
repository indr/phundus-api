using System.Collections.Generic;

namespace phiNdus.fundus.Web.Models.ArticleModels
{
    using Phundus.Core.Entities;

    public class ArticleReservationsModel
    {
        public ICollection<Reservation> Items { get; set; }
    }
}