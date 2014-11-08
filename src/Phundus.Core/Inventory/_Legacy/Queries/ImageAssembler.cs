namespace Phundus.Core.Inventory.Queries
{
    using System.Collections.Generic;
    using Domain.Model.Catalog;

    public class ImageAssembler
    {
        public ImageDto CreateDto(Image subject)
        {
            var result = new ImageDto();
            result.FileName = subject.FileName;
            result.Id = subject.Id;
            result.IsPreview = subject.IsPreview;
            result.Length = subject.Length;
            result.Type = subject.Type;
            result.Version = subject.Version;
            return result;
        }

        public IList<ImageDto> CreateDtos(Iesi.Collections.Generic.ISet<Image> subjects)
        {
            var result = new List<ImageDto>();
            foreach (var each in subjects)
                result.Add(CreateDto(each));
            return result;
        }
    }
}