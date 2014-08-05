namespace Phundus.Core.Inventory.Queries
{
    using System;
    using System.Collections.Generic;

    public class ArticleDto
    {
        IList<ImageDto> _images = new List<ImageDto>();

        public int Id { get; set; }

        public int Version { get; set; }

        public IList<ImageDto> Images
        {
            get { return _images; }
            set { _images = value; }
        }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public decimal Price { get; set; }

        public int GrossStock { get; set; }

        public string Description { get; set; }

        public string Specification { get; set; }

        public string Color { get; set; }

        public int OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        public void AddImage(ImageDto image)
        {
            Images.Add(image);
        }
    }
}