namespace phiNdus.fundus.Web.Business.Dto
{
    using System.Collections.Generic;

    public class ArticleDto : BasePropertiesDto
    {
        IList<ArticleDto> _children = new List<ArticleDto>();
        IList<ImageDto> _images = new List<ImageDto>();

        public int Id { get; set; }
        public int Version { get; set; }

        public IList<ArticleDto> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public IList<ImageDto> Images
        {
            get { return _images; }
            set { _images = value; }
        }

        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }

        public void AddChild(ArticleDto child)
        {
            Children.Add(child);
        }

        public void RemoveChild(ArticleDto child)
        {
            Children.Remove(child);
        }

        public void AddImage(ImageDto image)
        {
            Images.Add(image);
        }
    }
}