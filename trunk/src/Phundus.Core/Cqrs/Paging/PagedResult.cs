namespace Phundus.Core.Cqrs.Paging
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;

    /// <summary>
    /// Liefert ein "gepagtes" Resultat für einen bestimmten Typ.
    /// </summary>
    public class PagedResult<TItems>
    {
        public PagedResult(PageResponse pageResponse, ICollection<TItems> items)
        {
            Guard.Against<ArgumentNullException>(pageResponse == null, "pageResponse");
            Guard.Against<ArgumentNullException>(items == null, "items");

            Pages = pageResponse;
            Items = items;
        }

        public PagedResult()
        {
            Pages = new PageResponse();
            Items = new TItems[0];
        }

        public PageResponse Pages { get; private set; }

        /// <summary>
        /// Die Items für die angegebene Konfiguration.
        /// </summary>
        public ICollection<TItems> Items { get; private set; }
    }
}