namespace Phundus.Migrations
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web.Hosting;
    using FluentMigrator;

    [Migration(201603020156)]
    public class M201603020156_Migrate_article_images_directory_to_guids : DataMigrationBase
    {
        private readonly string _basePath = HostingEnvironment.MapPath(@"~\Content\Images\Articles");

        protected override void Migrate()
        {
            var command = CreateCommand("SELECT [Id], [ArticleGuid] FROM [Dm_Inventory_Article]");
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var shortId = reader.GetInt32(0);
                    var guid = reader.GetGuid(1);

                    RenameDirectory(shortId, guid);
                }
            }
        }

        private void RenameDirectory(int shortId, Guid guid)
        {
            var sourceDirName = Path.Combine(_basePath, shortId.ToString(CultureInfo.InvariantCulture));
            var destDirName = Path.Combine(_basePath, guid.ToString("D"));
            Directory.Move(sourceDirName, destDirName);
        }
    }
}