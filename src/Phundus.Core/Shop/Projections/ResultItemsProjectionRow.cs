namespace Phundus.Shop.Projections
{
    using System;

    public class ResultItemsProjectionRow
    {
        public virtual Guid RowId { get; set; }

        public virtual Guid ItemId { get; set; }
        public virtual int ItemShortId { get; set; }
        
        public virtual DateTime CreatedAtUtc { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal PublicPrice { get; set; }
        public virtual decimal? MemberPrice { get; set; }
        public virtual Guid OwnerGuid { get; set; }
        public virtual string OwnerName { get; set; }
        public virtual int OwnerType { get; set; }
        public virtual string PreviewImageFileName { get; set; }
    }
}