﻿namespace Phundus.Inventory.Articles.Model
{
    using Common.Domain.Model;

    public class Image : EntityBase
    {
        public Image()
        {
        }

        public Image(int id) : base(id)
        {
        }

        public virtual Article Article { get; set; }

        public virtual bool IsPreview { get; set; }

        public virtual long Length { get; set; }

        public virtual string Type { get; set; }

        public virtual string FileName { get; set; }
    }
}