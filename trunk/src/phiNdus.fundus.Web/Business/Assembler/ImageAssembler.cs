﻿namespace phiNdus.fundus.Web.Business.Assembler
{
    using System.Collections.Generic;
    using phiNdus.fundus.Web.Business.Dto;
    using Phundus.Core.InventoryCtx;
    using Phundus.Core.InventoryCtx.Model;

    public class ImageAssembler
    {
        public Image CreateDomainObject(ImageDto subject)
        {
            var result = new Image();
            result.IsPreview = subject.IsPreview;
            result.Length = subject.Length;
            result.Type = subject.Type;
            result.FileName = subject.FileName;
            return result;
        }

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