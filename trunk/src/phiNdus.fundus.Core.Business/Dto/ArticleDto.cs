using System;
using System.Collections.Generic;

namespace phiNdus.fundus.Core.Business.Dto
{
    public class ArticleDto : BasePropertiesDto
    {
        private IList<ArticleDto> _children = new List<ArticleDto>();
        private IList<ImageDto> _images = new List<ImageDto>();
        
        public int Id { get; set; }
        public int Version { get; set; }

        public IList<ArticleDto> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public void AddChild(ArticleDto child)
        {
            Children.Add(child);
        }

        public void RemoveChild(ArticleDto child)
        {
            Children.Remove(child);
        }

        public IList<ImageDto> Images
        {
            get { return _images; }
            set { _images = value; }
        }

        public void AddImage(ImageDto image)
        {
            Images.Add(image);
        }
    }
}