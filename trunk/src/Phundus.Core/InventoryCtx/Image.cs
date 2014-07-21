namespace Phundus.Core.InventoryCtx
{
    using Ddd;

    public class Image : EntityBase
    {
        public virtual Article Article { get; set; }

        public virtual bool IsPreview { get; set; }

        public virtual long Length { get; set; }

        public virtual string Type { get; set; }

        public virtual string FileName { get; set; }
    }
}