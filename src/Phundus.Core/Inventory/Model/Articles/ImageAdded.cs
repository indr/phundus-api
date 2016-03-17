namespace Phundus.Inventory.Articles.Model
{
    using System;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using Inventory.Model;

    [DataContract]
    public class ImageAdded : DomainEvent
    {
        public ImageAdded(Manager initiator, ArticleShortId articleShortId, ArticleId articleId, OwnerId ownerId,
            string fileName, string fileType, long fileLength, bool isPreviewImage)
        {
            if (initiator == null) throw new ArgumentNullException("initiator");
            if (articleShortId == null) throw new ArgumentNullException("articleShortId");
            if (articleId == null) throw new ArgumentNullException("articleId");
            if (ownerId == null) throw new ArgumentNullException("ownerId");
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (fileType == null) throw new ArgumentNullException("fileType");

            Initiator = initiator.ToActor();
            ArticleShortId = articleShortId.Id;
            ArticleId = articleId.Id;
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
        public Actor Initiator { get; protected set; }

        [DataMember(Order = 2)]
        public int ArticleShortId { get; protected set; }

        [DataMember(Order = 3)]
        public Guid ArticleId { get; protected set; }

        [DataMember(Order = 4)]
        public Guid OwnerId { get; protected set; }

        [DataMember(Order = 5)]
        public string FileName { get; protected set; }

        [DataMember(Order = 6)]
        public string FileType { get; protected set; }

        [DataMember(Order = 7)]
        public long FileLength { get; protected set; }

        [DataMember(Order = 8)]
        public bool IsPreviewImage { get; protected set; }
    }
}