﻿namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class PreviewImageChanged : DomainEvent
    {
        public PreviewImageChanged(Initiator initiator, ArticleId articleId, ArticleGuid articleGuid, OwnerId ownerId,
            string fileName, string fileType, long fileLength)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (articleGuid == null) throw new ArgumentNullException("articleGuid");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (fileType == null) throw new ArgumentNullException("fileType");
            Initiator = initiator;
            ArticleIntegralId = articleId.Id;
            ArticleGuid = articleGuid.Id;
            OwnerId = ownerId.Id;
            FileName = fileName;
            FileType = fileType;
            FileLength = fileLength;
        }

        protected PreviewImageChanged()
        {
        }

        [DataMember(Order = 1)]
        public Initiator Initiator { get; set; }

        [DataMember(Order = 2)]
        public int ArticleIntegralId { get; set; }

        [DataMember(Order = 3)]
        public Guid ArticleGuid { get; set; }

        [DataMember(Order = 4)]
        public Guid OwnerId { get; set; }

        [DataMember(Order = 5)]
        public string FileName { get; set; }

        [DataMember(Order = 6)]
        public string FileType { get; set; }

        [DataMember(Order = 7)]
        public long FileLength { get; set; }
    }
}