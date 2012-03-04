﻿using System;
using System.Collections.Generic;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.Core.Business.Assembler
{
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