﻿using System.Collections.Generic;

namespace phiNdus.fundus.Web.Models.ArticleModels
{
    using Phundus.Core.ReservationCtx;

    public class ArticleReservationsModel
    {
        public ICollection<Reservation> Items { get; set; }
    }
}