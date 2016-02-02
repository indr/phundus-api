namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;

    [DataContract]
    public class ImageAdded : DomainEvent
    {
        public ImageAdded(Initiator initiator, ArticleId articleIntegralId, ArticleGuid articleGuid, OwnerId ownerId,
            string fileName, string fileType, long fileLength, bool isPreviewImage)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (articleIntegralId == null) throw new ArgumentNullException("articleIntegralId");
            if (articleGuid == null) throw new ArgumentNullException("articleGuid");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (fileType == null) throw new ArgumentNullException("fileType");
            Initiator = initiator;
            ArticleIntegralId = articleIntegralId.Id;
            ArticleGuid = articleGuid.Id;
            OwnerId = ownerId.Id;
            FileName = fileName;
            FileType = fileType;
            FileLength = fileLength;
            IsPreviewImage = isPreviewImage;
        }

        protected ImageAdded()
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

        [DataMember(Order = 8)]
        public bool IsPreviewImage { get; set; }
    }
}