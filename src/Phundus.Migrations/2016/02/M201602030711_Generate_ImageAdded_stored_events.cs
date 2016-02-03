namespace Phundus.Migrations
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;
    using Common.Domain.Model;
    using FluentMigrator;

    [Migration(201602030711)]
    public class M201602030711_Generate_ImageAdded_stored_events : EventMigrationBase
    {
        protected override void Migrate()
        {
            var command = CreateCommand(@"SELECT [ArticleGuid], [Owner_OwnerId], [CreateDate], [ArticleId], [IsPreview], [Length], [Type], [FileName] FROM [Dm_Inventory_ArticleFile] af
INNER JOIN [Dm_Inventory_Article] a ON a.Id = af.ArticleId
ORDER BY [ArticleId] ASC, [IsPreview] DESC, [Type] DESC");


            var lastArticleId = 0;
            var articleHasPreview = false;
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                {
                    var articleGuid = reader.GetGuid(0);
                    var ownerGuid = reader.GetGuid(1);
                    var createdAtUtc = reader.GetDateTime(2).ToUniversalTime();
                    var articleId = reader.GetInt32(3);
                    var isPreview = reader.GetBoolean(4);
                    var fileLength = reader.GetInt64(5);
                    var fileType = reader.GetString(6);
                    var fileName = reader.GetString(7);

                    if (lastArticleId != articleId)
                    {
                        articleHasPreview = isPreview;
                    }

                    if (!articleHasPreview && fileType.StartsWith("image", true, CultureInfo.InvariantCulture))
                    {
                        isPreview = true;
                        articleHasPreview = true;
                    }


                    var domainEvent = new ImageAdded_20160203
                    {
                        ArticleGuid = articleGuid,
                        ArticleIntegralId = articleId,
                        FileLength = fileLength,
                        FileName = fileName,
                        FileType = fileType,
                        Initiator = null,
                        IsPreviewImage = isPreview,
                        OwnerId = ownerGuid
                    };

                    InsertStoredEvent(createdAtUtc.AddMinutes(1), "Phundus.Inventory.Articles.Model.ImageAdded, Phundus.Core", domainEvent);

                    lastArticleId = articleId;
                }
        }

        [DataContract]
        public class ImageAdded_20160203 : DomainEvent
        {
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
}